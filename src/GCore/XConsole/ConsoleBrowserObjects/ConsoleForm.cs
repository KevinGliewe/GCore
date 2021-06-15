using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// The root object of the hierarchy.  Contains enough information to render
    /// labels (read-only) and textboxes (read/write) to a console screen.  It manages
    /// interaction with the keyboard to solicit input to fields, navigation through
    /// the fields, and completion of the form.
    /// </summary>
    public class ConsoleForm : IXmlSerializable, 
                               IDisposable {
        #region Event Constructs
        /// <summary>
        /// A delegate to a method that handles the event raised when a key is pressed
        /// on a <see cref="ConsoleForm">ConsoleForm</see>.
        /// </summary>
        /// <param name="sender">The <see cref="ConsoleForm">ConsoleForm</see> in which the
        /// key was pressed.</param>
        /// <param name="e">A <see cref="KeyPressEventArgs">KeyPressEventArgs</see> object
        /// that contains the key being pressed, the field in which it was pressed, and a 
        /// property settable by the subscriber to cancel the processing of the key.</param>
        public delegate void onKeyPress(ConsoleForm sender, KeyPressEventArgs e);

        /// <summary>
        /// Represents the method that will be invoked when a form is submitted.
        /// </summary>
        /// <remarks>Usually, the type of the sender variable is an object, 
        /// but this delegate will only be called from a 
        /// <see cref="ConsoleForm">ConsoleForm</see>, so we can afford to use
        /// the more specific type.  The <see cref="EventArgs">System.EventArgs e</see>
        /// is not used.</remarks>
        public   delegate void  onFormComplete(ConsoleForm sender, FormCompleteEventArgs e);

        /// <summary>
        /// Represents the method that will be invoked when a form is cancelled.
        /// </summary>
        /// <remarks>Usually, the type of the sender variable is an 
        /// <see cref="Object">object</see>, but this delegate will only be called
        /// from a <see cref="ConsoleForm">ConsoleForm</see>, so we can afford to use
        /// the more specific type.  The <see cref="EventArgs">System.EventArgs e</see>
        /// is not used.</remarks>
        public   delegate void  onFormCancelled(ConsoleForm sender, System.EventArgs e);

        private onKeyPress _keyPressEvent = null;

        /// <summary>
        /// Event raised when any key is pressed in a field. It allows a subscriber to cancel
        /// the keypress before it is processed.  Only 1 event handler can be wired to this
        /// event.
        /// </summary>
        public event onKeyPress KeyPressed {
            add {
                if (_keyPressEvent == null)
                    _keyPressEvent = value;
                else
                    throw new InvalidOperationException("Can only wire 1 handler to this event.");
            }
            remove {
                if (_keyPressEvent == value)
                    _keyPressEvent = null;
                else
                    throw new InvalidOperationException("You can't unhook an unwired event.");
            }
        }

        private onFormComplete _formCompleteEvent = null;
        /// <summary>
        /// An event that is raised when the Enter key is pressed on the form.
        /// </summary>
        public event onFormComplete FormComplete {
            add {
                if (_formCompleteEvent == null)
                    _formCompleteEvent = value;
                else
                    throw new InvalidOperationException("Can only wire 1 handler to this event.");
            }
            remove {
                if (_formCompleteEvent == value)
                    _formCompleteEvent = null;
                else
                    throw new InvalidOperationException("You can't unhook an unwired event.");
            }
        }

        /// <summary>
        /// An event that is raised when the Esc key is pressed on the form.
        /// </summary>
        public  event       onFormCancelled FormCancelled;
        #endregion

        #region Private Member Variables
        private     Labels      _labels         = new Labels();      // Collection of label objects
        private     Textboxes   _textboxes      = new Textboxes();   // Collection of textbox objects
        private     Lines       _lines          = new Lines();       // Collection of line objects
        private     string      _name           = string.Empty;      // Name of the form
        private     Int32       _currentField   = 0;                 // Field index currently taking input
        private     Int32       _width          = 80;                // Default width
        private     Int32       _height         = 40;                // Default height
        private     Textbox     _field          = null;              // Textbox with focus
        private     Thread      _keyThread      = null;              // The thread that waits for keypresses
        private     ThreadStart _keyThreadStart = null;              // Parameters for the keypress thread
        #endregion
		
        #region ctors
        /// <summary>
        /// Initializes an instance of a ConsoleForm.
        /// </summary>
        private   ConsoleForm() {
            // Initialize the keypress thread variables.
            _keyThreadStart = new ThreadStart(LoopForKeypress);
            _keyThread = new Thread(_keyThreadStart);
        }
        /// <summary>
        /// Initializes a new ConsoleForm of a specified height and width.  The only
        /// constructor available to external clients.
        /// </summary>
        /// <param name="width">Width in characters of the new screen.</param>
        /// <param name="height">Height in characters of the new screen.</param>
        public ConsoleForm(Int32 width, Int32 height) : this() {
            _width = width;
            _height = height;
        }
        #endregion

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ConsoleForm() {
        }

        #region Static Factory Methods
        /// <summary>
        /// Method to be called when a new <see cref="ConsoleForm">ConsoleForm</see> is 
        /// needed to be deserialized from disk.
        /// </summary>
        /// <param name="path">A string with the fully qualified path to the 
        /// form definition file on disk.  If the path starts with ".\", the function
        /// will replace the "." with the path of the executing assembly.</param>
        /// <returns>A new instance of a console form.</returns>
        public static  ConsoleForm GetFormInstance(string path) {
            ConsoleForm       form     = new ConsoleForm();
            XmlSerializer     ser      = new XmlSerializer(form.GetType());

            if (path.IndexOf(".\\") == 0)
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + path.Substring(1);

            using (StreamReader sr = new StreamReader(path)) {
                form = (ConsoleForm)ser.Deserialize(sr);

                sr.Close();
            }

            return form;
        }

        /// <summary>
        /// Method to be called when a new console form is needed to be 
        /// deserialized from disk, and the functions to be called on
        /// completion and on cancellation are known.
        /// </summary>
        /// <param name="path">A string with the fully qualified path to the 
        /// form definition file on disk.</param>
        /// <param name="formComplete">Delegate to a function to be
        /// called when the form is completed.</param>
        /// <param name="formCancelled">Delegate to a function to be called
        /// when the form is cancelled.  Can be null.</param>
        /// <returns>A new instance of a console form with properties set and
        /// appropriate events wired up.  Can be null.</returns>
        public static  ConsoleForm GetFormInstance
                                    (string            path, 
                                    onFormComplete    formComplete, 
                                    onFormCancelled   formCancelled) {
            // Call the other static factory method to get a new console form.
            ConsoleForm       form     = ConsoleForm.GetFormInstance(path);

            if (formComplete != null)
                form.FormComplete += formComplete;

            if (formCancelled != null)
                form.FormCancelled += formCancelled;

            return form;
        }
        #endregion

        /// <summary>
        /// Method called to update the interface when the data in a textbox or 
        /// a label is modified.
        /// </summary>
        /// <param name="sco"></param>
        private void Refresh(StdConsoleObject sco) {
            // Mark the starting position and colour of the console.
            Int32 x = Console.CursorLeft;
            Int32 y = Console.CursorTop;

            ConsoleColor fore = Console.ForegroundColor;
            ConsoleColor back = Console.BackgroundColor;

            // Make sure the data being written to the screen is either
            // truncated if too long, or padded if too short, to make the
            // field being shown appear correct.
            string text = string.Empty;

            // Make sure to refresh with password masking characters if this is
            // a password field.
            if (sco is Textbox && ((Textbox)sco).PasswordChar != char.MinValue)
                text = new string(((Textbox)sco).PasswordChar, sco.Text.Length);
            else
                text = sco.Text;

            if (text.Length > sco.Length)
                text = text.Substring(0, sco.Length);

            if (text.Length < sco.Length)
                text = text.PadRight(sco.Length, ' ');

            // Actually write the text
            Console.SetCursorPosition(sco.Location.X, sco.Location.Y);
            Console.BackgroundColor = sco.Background;
            Console.ForegroundColor = sco.Foreground;
            Console.Write(text);

            // Reset the cursor and colour information.
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
            Console.SetCursorPosition(x, y);

            // If the field being updated is the "current" field (i.e. the one with the 
            // cursor in it), reposition the cursor to accomodate existing text.
            if (sco is Textbox)
                if (((Textbox)sco) == _field)
                    Console.SetCursorPosition(_field.Location.X + _field.Text.Length, 
                                              _field.Location.Y);
        }

        #region Public Methods
        public      void        SetFocus(Textbox field) {
            for (Int32 i = 0; i < _textboxes.Count; i++)
                if (_textboxes[i] == field) {
                    _field = Textboxes[i];
                    _currentField = i;

                    Console.ForegroundColor = _field.Foreground;
                    Console.BackgroundColor = _field.Background;
                    Console.SetCursorPosition(_field.Location.X + _field.Text.Length, _field.Location.Y);

                    return;
                }

            throw new InvalidOperationException(field.Name + " not found.");
        }

        /// <summary>
        /// Draws the current form onto the console.
        /// </summary>
        public void Render() {
            this.Render(true);
        }

        /// <summary>
        /// Draws the current form onto the console.
        /// </summary>
        /// <param name="clear">A <see cref="bool">bool</see> indicating whether or
        /// not the screen should be cleared before it is redrawn.</param>
        public      void        Render(bool clear) {
            Console.ResetColor();
            
            if (clear)
                Console.Clear();

            Console.Title = _name;

            // Resize the window and the buffer to the form's size.
            if (Console.BufferHeight != _height || Console.BufferWidth != _width) {
                Console.SetWindowSize(_width, _height);
                Console.SetBufferSize(_width, _height);
            }

            if (Console.WindowHeight != _height || Console.WindowWidth != _width) {
                Console.SetBufferSize(_width, _height);
                Console.SetWindowSize(_width, _height);
            }

            // Draw the lines first.
            foreach (Line line in _lines) {
                Console.BackgroundColor = line.Colour;

                if (line.Orientation == Line.LineOrientation.Horizontal) {
                    // Instructions for drawing a horizontal line.
                    Console.SetCursorPosition(line.Location.X, line.Location.Y);
                    Console.Write(new string(' ', line.Length));
                } else {
                    // Instructions for drawing a vertical line.
                    Int32 x = line.Location.X;

                    for (Int32 i = line.Location.Y; i < line.Location.Y + line.Length; i++) {
                        Console.SetCursorPosition(x, i);
                        Console.Write(" ");
                    }
                }
            }

            // Draw the labels next.
            foreach (Label label in _labels)
                Refresh(label);

            // Now draw the textboxes.
            foreach (Textbox text in _textboxes)
                Refresh(text);

            // If any textboxes are defined for the form, pick the first one and position
            // the cursor accordingly.
            if (_textboxes.Count > 0) {
                _field = _textboxes[0];
                _textboxes.FocusField = _field;
                Console.SetCursorPosition(_field.Location.X + _field.Text.Length, _field.Location.Y);
                Console.CursorVisible = true;
            } else
                // Otherwise, hide the cursor.
                Console.CursorVisible = false;

            _labels.Rendered();
            _textboxes.Rendered();

            if (_keyThread.Name == null) {
                // Start the thread that listens for keypresses.
                _keyThread.Name = "Keypress loop for " + _name;
                _keyThread.Start();
            }
        }

        /// <summary>
        /// A method that loops and processes keystrokes until the form is complete, 
        /// either by <see cref="FormComplete">FormComplete</see> or 
        /// <see cref="FormCancelled">FormCancelled</see> events.
        /// </summary>
        private void LoopForKeypress() {
            // Loop for keypresses.  Since we're doing all the work of processing, we have
            // to trap special keypresses and respond appropriately
            while (true) {
                // Blocks on the next function call.
                ConsoleKeyInfo cki = Console.ReadKey(true);

                ConsoleKey nKey = cki.Key;

                // A key's been pressed.  Figure out what to do.
                // All actions will be against the current field, stored in _field.
                char	cChar	= cki.KeyChar;

                if (cChar != 0) {        // Guard against unprintable chars.
                    KeyPressEventArgs kpea = new KeyPressEventArgs(_field, cChar);

                    if (_keyPressEvent != null)
                        _keyPressEvent(this, kpea);

                    if (!kpea.Cancel) {     // Process the keystroke.  It wasn't cancelled.
                        switch (nKey) {
                            case ConsoleKey.Backspace:		// Backspace pressed
                                // Is there a character to backspace over?
                                if (_field.Text.Length > 0) {
                                    _field.Text = _field.Text.Substring(0, _field.Text.Length - 1);
                                    Refresh(_field);
                                }

                                break;

                            case ConsoleKey.Tab:		// Tab -> Move to the next field.
                                if (cki.Modifiers == ConsoleModifiers.Shift) { 
                                    // Go backwards.
                                    _currentField--;

                                    // If we're at the first field, move to the last.
                                    if (_currentField == -1)
                                        _currentField = _textboxes.Count - 1;
                                } else {
                                    // Go forwards
                                    _currentField++;

                                    // If we're in the last field already, move back to the first.
                                    if (_currentField == _textboxes.Count)
                                        _currentField = 0;
                                }

                                // Set the current field to the next one in the collection.
                                _field = _textboxes[_currentField];
                                _textboxes.FocusField = _field;

                                // Move the cursor to the location of the next field, accomodating
                                // any text that may already be there..
                                Console.SetCursorPosition(_field.Location.X + _field.Text.Length, _field.Location.Y);
                                break;

                            case ConsoleKey.Enter:		// Enter -> Fire the complete event if it's wired.
                                if (_formCompleteEvent != null) {
                                    FormCompleteEventArgs fcea = new FormCompleteEventArgs();

                                    _formCompleteEvent(this, fcea);

                                    // The listener of this event will set the Cancel field if they
                                    // want to re-use the form.  If not cancelled, the form will
                                    // be destroyed.
                                    if (!fcea.Cancel) {
                                        // Get rid of this form.  A new one will be displayed.

                                        // Unusual to call Dispose() on oneself, but it saves a lot of
                                        // code in the clients if this is the default behaviour, rather
                                        // than forcing every single event to call Dispose() on the
                                        // passed-in sender parameter.
                                        ((IDisposable)this).Dispose();
                                        return;
                                    }   // else the current form will be reused.  Go back for more keys.
                                }

                                break;

                            case ConsoleKey.Escape:     // Esc -> Fire the cancelled event if it's wired.
                                if (this.FormCancelled != null) {
                                    this.FormCancelled(this, System.EventArgs.Empty);

                                    ((IDisposable)this).Dispose();
                                    return;
                                }

                                break;

                            default:		// Any other keystroke
                                if (_field != null) {   // May not be an active textbox.
                                    if (_field.Text.Length < _field.Length) {
                                        // The field is not yet full.  It can be appended to.
                                        _field.NonEventingText += cChar;
                                        Console.ForegroundColor = _field.Foreground;
                                        Console.BackgroundColor = _field.Background;

                                        if (_field.PasswordChar != char.MinValue)
                                            // It's a password field.  Display the password character.
                                            Console.Write(_field.PasswordChar);
                                        else
                                            // Not a password type field.  Show the actual character.
                                            Console.Write(cChar);
                                    }  // Field already full => no keystrokes accepted.
                                }
                                break;
                        } // Keystroke was not cancelled
                    } // Character was printable
                }  // End of switch statement
            }  // End loop for keypresses
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Property used to determine the current width of the screen. The value is 
        /// immutable once set by the constructor.
        /// </summary>
        public Int32 Width { get { return _width; } }

        /// <summary>
        /// Property used to determine the current height of the screen. The value is
        /// immutable once set by the constructor.
        /// </summary>
        public Int32 Height { get { return _height; } }

        /// <summary>
        /// A <see cref="string">string</see> property holding the name of 
        /// the console screen.
        /// </summary>
        public      string      Name {
            get {return _name;}
            set {_name = value;}
        }

        /// <summary>
        /// A collection of <see cref="Line">Line</see> objects on the form.
        /// </summary>
        public      Lines       Lines {
            get {return _lines;}
        }

        /// <summary>
        /// A collection of <see cref="Label">Label</see> objects on the form.
        /// </summary>
        public      Labels      Labels {
            get {return _labels;}
        }

        /// <summary>
        /// The collection of <see cref="Textbox">Textbox</see> objects on the form.
        /// </summary>
        public      Textboxes   Textboxes {
            get {return _textboxes;}
        }
        #endregion
	
        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the 
        /// <see cref="ConsoleForm">ConsoleForm</see> object.</summary>
        /// <param name="writer">The stream to which this object will be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteAttributeString(string.Empty, "Name", string.Empty, _name);

            ((IXmlSerializable)_lines).WriteXml(writer);
            ((IXmlSerializable)_labels).WriteXml(writer);
            ((IXmlSerializable)_textboxes).WriteXml(writer);
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        /// <returns></returns>
        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema() {
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="ConsoleForm">ConsoleForm</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            // Read the name of the form.
            _name  = reader.GetAttribute("Name");

            // Get the width and height of the form, if they're specified.
            if (reader.GetAttribute("Width") != null)
                _width = Int32.Parse(reader.GetAttribute("Width"));

            if (reader.GetAttribute("Height") != null)
                _height = Int32.Parse(reader.GetAttribute("Height"));

            // Move to the node after the <ConsoleForm> node.
            reader.Read();
            
            // Expect to see a node of Line objects.
            if (reader.Name == "Lines")
                ((IXmlSerializable)_lines).ReadXml(reader);
            else
                throw new InvalidOperationException("<Lines> element missing from form definition.");

            // Now expect to see a node containing the Label objects.
            if (reader.Name == "Labels")
                ((IXmlSerializable)_labels).ReadXml(reader);
            else
                throw new InvalidOperationException("<Labels> element missing from form definition.");

            // Finally, we expect to see the node containing the Textbox objects.
            if (reader.Name == "Textboxes")
                ((IXmlSerializable)_textboxes).ReadXml(reader);
            else
                throw new InvalidOperationException("<Textboxes> element missing from form definition.");
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="ConsoleForm">ConsoleForm</see>
        /// object.
        /// </summary>
        void IDisposable.Dispose() {
            ((IDisposable)_labels).Dispose();
            ((IDisposable)_textboxes).Dispose();
            ((IDisposable)_lines).Dispose();

            // Unwire any listening events.
            _keyPressEvent = null;
            _formCompleteEvent = null;
            FormCancelled = null;

            // Terminate the keypress loop.
            _keyThread.Abort();
            _keyThread = null;

            GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// A class used to communicate the results of a FormComplete event to 
    /// subscribed clients.  More importantly, it allows the subscribed client
    /// to communicate back to the ConsoleForm whether or not the old form should
    /// be disposed or not.
    /// </summary>
    public class FormCompleteEventArgs : System.EventArgs {
        #region Private Members
        private bool _cancel = false;
        #endregion

        #region Public Properties
        /// <summary>
        /// Property to set to prevent the ending of the keypress loop, and the disposal
        /// of form resources.  If true, the form will not be disposed and the keypress loop
        /// thread will continue to wait for keypresses.  If false (default), the form
        /// will be disposed at the end of the FormComplete event handler.
        /// </summary>
        public bool Cancel {
            get { return _cancel; }
            set { _cancel = value; }
        }
        #endregion
    }

    /// <summary>
    /// Class to provide parameters to subscribers to KeyPressed events.
    /// </summary>
    public class KeyPressEventArgs : System.EventArgs {
        #region Private Members
        private bool        _cancel = false;
        private char        _char   = char.MinValue;
        private Textbox     _field  = null;
        #endregion

        #region ctors
        /// <summary>
        /// Constructor called to initialize the event args with the key pressed by the user.
        /// </summary>
        /// <param name="c"></param>
        public KeyPressEventArgs(Textbox field, char c) {
            _field = field;
            _char = c;
        }
        #endregion

        #region Public Properties
        public Textbox Textbox { get { return _field; } }

        /// <summary>
        /// Property that exposes the key pressed by the user to event subscribers, so the
        /// decision can be made about whether to cancel it or not.
        /// </summary>
        public char Char { get { return _char; } }

        /// <summary>
        /// Property settable by event subscribers to cancel processing of the keypress.
        /// True means cancel the keypress.  False (default) means don't cancel the 
        /// keypress.  Process it.
        /// </summary>
        public bool Cancel {
            get { return _cancel; }
            set { _cancel = value; }
        }
        #endregion
    }
}