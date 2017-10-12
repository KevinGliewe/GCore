using System;
using System.Xml;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// The base class for textual information displayed by the console.
    /// </summary>
    public abstract class StdConsoleObject : System.Xml.Serialization.IXmlSerializable,
                                             IDisposable {

        #region Private Member Variables
        private     ConsoleColor _foreground;
        private     ConsoleColor _background;

        private     Point       _location      = new Point();

        private     string      _name          = string.Empty;
        private     Int32       _length        = 0;
        protected   bool        _rendered      = false;
        protected   string      _text          = string.Empty;
        #endregion
		
        #region ctors
        internal StdConsoleObject() { }

        /// <summary>
        /// Constructor called when the Name, Location and Length of the StdConsoleObject are known
        /// at instantiation-time. These are the minimal parameters required for instantiation
        /// by a client, since they are immutable once created. 
        /// </summary>
        /// <param name="name">The name of the StdConsoleObject for accessing it in its collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the <see cref="StdConsoleObject">StdConsoleObject</see>
        /// should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        public		StdConsoleObject
                    (string      name,
                     Point       location,
                     Int32       length) {
            _name = name;
            _location = location;
            _length = length;
        }

        /// <summary>Constructor called when the Name, Location, Length and Text of the label are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. 
        /// </summary>
        /// <param name="name">The name of the StdConsoleObject for accessing it in its collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the StdConsoleObject should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="StdConsoleObject">StdConsoleObject</see>.
        /// will display.</param>
        public      StdConsoleObject
                    (string         name, 
                     Point          location,
                     Int32          length, 
                     string         text) : this(name, location, length) {
            _text = text;
        }

        /// <summary>Constructor called when the Name, Location, Length, Text and colours of the 
        /// <see cref="StdConsoleObject">StdConsoleObject</see> are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. 
        /// </summary>
        /// <param name="name">The name of the <see cref="StdConsoleObject">StdConsoleObject</see> for 
        /// accessing it in its collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the label should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="Label">Label</see>
        /// will display.</param>
        /// <param name="foreground">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the foreground colour of the text of the 
        /// <see cref="StdConsoleObject">StdConsoleObject</see>.</param>
        /// <param name="background">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the background colour of the of the 
        /// <see cref="StdConsoleObject">StdConsoleObject</see>.</param>
        public      StdConsoleObject
                    (string name,
                     Point location,
                     Int32 length,
                     string text,
                     ConsoleColor foreground,
                     ConsoleColor background) : this(name, location, length, text) {
            _foreground = foreground;
            _background = background;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The colour data used to render the foreground of the text.
        /// </summary>
        public ConsoleColor Foreground {
            get {return _foreground;}
            set {_foreground = value;}
        }
		
        /// <summary>
        /// The colour data used to display the background of the text.
        /// </summary>
        public ConsoleColor Background {
            get {return _background;}
            set {_background = value;}
        }
		
        /// <summary>
        /// A location for the field on the console.
        /// </summary>
        public		Point   Location {
            get {return _location;}
        }
		
        /// <summary>
        /// The name of the console object, used to access it programmatically
        /// in a collection.
        /// </summary>
        public  string      Name {
            get {return _name;}
        }
		
        /// <summary>
        /// The text to be displayed.
        /// </summary>
        public  string      Text {
            get {return _text;}
            set {
                if (_text != value) {
                    _text = value;

                    if (_rendered) {
                        // Mark the starting position and colour of the console.
                        Int32 x = Console.CursorLeft;
                        Int32 y = Console.CursorTop;

                        ConsoleColor fore = Console.ForegroundColor;
                        ConsoleColor back = Console.BackgroundColor;

                        // Make sure the data being written to the screen is either
                        // truncated if too long, or padded if too short, to make the
                        // field being shown appear correct.
                        string text = _text;
                        if (text.Length > _length)
                            text = text.Substring(0, _length);

                        if (text.Length < _length)
                            text = text.PadRight(_length, ' ');

                        // Actually write the text
                        Console.SetCursorPosition(_location.X, _location.Y);
                        Console.BackgroundColor = _background;
                        Console.ForegroundColor = _foreground;
                        Console.Write(text);

                        // Reset the cursor and colour information.
                        Console.ForegroundColor = fore;
                        Console.BackgroundColor = back;
                        Console.SetCursorPosition(x, y);

                        if (this is Textbox)
                            if (((Textbox)this).Focus)
                                Console.SetCursorPosition(_location.X + _text.Length, _location.Y);
                    }
                }
            }
        }
		
        /// <summary>
        /// The maximum length of the field to be shown.
        /// </summary>
        public  Int32       Length {
            get {return _length;}
        }
        #endregion

        #region Internal Methods
        internal void Rendered() {
            _rendered = true;
        }
        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="StdConsoleObject">StdConsoleObject</see>.</summary>
        /// <param name="writer">The stream to which this object will
        /// be serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteAttributeString(string.Empty, "Name",        string.Empty, _name);
            writer.WriteAttributeString(string.Empty, "Text",        string.Empty, _text);
            writer.WriteAttributeString(string.Empty, "Length",      string.Empty, _length.ToString());
            writer.WriteAttributeString(string.Empty, "ForeColour",  string.Empty, _foreground.ToString());
            writer.WriteAttributeString(string.Empty, "BackColour",  string.Empty, _background.ToString());

            ((IXmlSerializable)_location).WriteXml(writer);
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema() {
            // TODO:  Add StdConsoleObject.GetSchema implementation
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="StdConsoleObject">StdConsoleObject</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader) {
            _name = reader.GetAttribute("Name");
            _text = (reader.GetAttribute("Text") == null ? string.Empty : reader.GetAttribute("Text"));
            
            string   length = reader.GetAttribute("Length");
            if (length != null && length.Length > 0)
                _length = Int32.Parse(length);
            else
                _length = _text.Length;

            string   foreground  = reader.GetAttribute("ForeColour");
            if (foreground != null && foreground.Length > 0)
                _foreground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foreground);

            string   background  = reader.GetAttribute("BackColour");
            if (background != null && background.Length > 0)
                _background = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), background);

            reader.Read();    

            if (reader.Name == "Location")
                ((IXmlSerializable)_location).ReadXml(reader);
            else
                throw new InvalidOperationException("<Location> node missing from " + _name + " node.");
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="StdConsoleObject">StdConsoleObject</see>
        /// object.
        /// </summary>
        public void Dispose() {
            ((IDisposable)_location).Dispose();

            GC.SuppressFinalize(this);
        }
        #endregion
   }
}