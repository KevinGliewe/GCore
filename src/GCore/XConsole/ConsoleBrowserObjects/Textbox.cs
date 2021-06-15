using System;
using System.Xml;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// An object used to manage the data necessary to render read-write
    /// objects to the screen.
    /// </summary>
    public class Textbox : GCore.XConsole.ConsoleBrowserObjects.StdConsoleObject, System.Xml.Serialization.IXmlSerializable {
        #region Private Members
        private  ConsoleColor    _DEFAULT_BACKGROUND     = ConsoleColor.White;
        private  ConsoleColor    _DEFAULT_FOREGROUND     = ConsoleColor.Black;

        private  char            _passwordChar           = char.MinValue;
        private  bool            _focus                  = false;
        #endregion

        #region ctors
        /// <summary>
        /// Constructor callable by other members of the namespace when a new object is
        /// going to be deserialized into it.
        /// </summary>
        internal    Textbox() {
            this.InitializeColours();
        }

        /// <summary>
        /// Constructor called when the Name, Location and Length of the textbox are known
        /// at instantiation-time. These are the minimal parameters required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.
        /// </summary>
        /// <param name="name">The name of the Textbox for accessing it in the Textboxes collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the Textbox should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        public      Textbox
                    (string         name,
                     Point          location,
                     Int32          length) : base(name, location, length) {
            this.InitializeColours();
        }

        /// <summary>Constructor called when the Name, Location, Length and Text of the textbox are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.
        /// </summary>
        /// <param name="name">The name of the textbox for accessing it in the collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the label should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="Label">Label</see>
        /// will display.</param>
        public      Textbox
                    (string         name,
                     Point          location,
                     Int32          length, 
                     string         text) : base(name, location, length, text) {
            this.InitializeColours();
        }

        /// <summary>Constructor called when the Name, Location, Length, Text and colours of the Textbox are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.  It does not call InitializeColours() since
        /// the colours are being explicitly set, and we won't care about the defaults.
        /// </summary>
        /// <param name="name">The name of the Textbox for accessing it in the Textboxes collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the Textbox should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="Textbox">Textbox</see>
        /// will display.</param>
        /// <param name="foreground">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the foreground colour of the text of the <see cref="Textbox">Textbox</see>.</param>
        /// <param name="background">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the background colour of the of the <see cref="Textbox">Textbox</see>.</param>
        public      Textbox
                    (string         name,
                     Point          location,
                     Int32          length,
                     string         text, 
                     ConsoleColor   foreground,
                     ConsoleColor   background) : base(name, location, length, text, foreground, background) { }
        #endregion

        #region Private Methods
        private	void			InitializeColours() {
            this.Background = _DEFAULT_BACKGROUND;
            this.Foreground = _DEFAULT_FOREGROUND;
        }
        #endregion

        #region Internal Properties
        internal bool Focus {
            get { return _focus; }
            set { _focus = value; }
        }

        internal string NonEventingText {
            get { return base._text; }
            set { base._text = value; }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// A <see cref="bool">char</see> that defines the masking
        /// character for a password field.  If undefined, the field
        /// is a regular textbox.
        /// </summary>
        public      char     PasswordChar {
            get {return _passwordChar;}
            set {_passwordChar = value;}
        }
        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// Reads Xml when the <see cref="Textbox">Textbox</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(XmlReader reader) {
            string   password    = reader.GetAttribute("PasswordChar");

            if (password != null)
                _passwordChar = char.Parse(password);

            base.ReadXml(reader);
        }

        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Textbox">Textbox</see> object.
        /// </summary>
        /// <param name="writer">The stream to which this object will be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);

            if (_passwordChar != char.MinValue)
                writer.WriteAttributeString("PasswordChar", _passwordChar.ToString());

            base.WriteXml(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}