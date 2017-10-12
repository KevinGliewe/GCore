using System;
using System.Xml;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// A class used to store the data necessary for a write-only
    /// object rendered on the console screen.
    /// </summary>
    public class Label : GCore.XConsole.ConsoleBrowserObjects.StdConsoleObject, 
                         IXmlSerializable {
        #region Private Members
        private  ConsoleColor    _DEFAULT_BACKGROUND     = ConsoleColor.Black;
        private  ConsoleColor    _DEFAULT_FOREGROUND     = ConsoleColor.White;
        #endregion

        #region ctors
        /// <summary>
        /// Constructor available only to other ConsoleBrowserObjects so parameterless
        /// objects can be created, and XmlSerialized into.
        /// </summary>
        internal    Label() {
            this.InitializeColours();
        }

        /// <summary>
        /// Constructor called when the Name, Location and Length of the label are known
        /// at instantiation-time. These are the minimal parameters required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.
        /// </summary>
        /// <param name="name">The name of the label for accessing it in the Labels collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the label should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        public      Label
                    (string name,
                     Point location,
                     Int32 length) : base(name, location, length) {
            this.InitializeColours();
        }

        /// <summary>Constructor called when the Name, Location, Length and Text of the label are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.
        /// </summary>
        /// <param name="name">The name of the label for accessing it in the Labels collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the label should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="Label">Label</see>
        /// will display.</param>
        public      Label
                    (string     name,
                     Point      location, 
                     Int32      length, 
                     string     text) : base(name, location, length, text) {
            this.InitializeColours();
        }

        /// <summary>Constructor called when the Name, Location, Length, Text and colours of the label are known
        /// at instantiation-time. The first 3 parameters are required for instantiation
        /// by a client, since they are immutable once created. This method just invokes the base
        /// constructor with the supplied parameters.  It does not call InitializeColours() since
        /// the colours are being explicitly set, and we won't care about the defaults.
        /// </summary>
        /// <param name="name">The name of the label for accessing it in the Labels collection.</param>
        /// <param name="location">A <see cref="Point">Point</see> object that describes
        /// where on the console screen the label should be rendered.</param>
        /// <param name="length">The length of the field as an <see cref="Int32">Int32</see>.</param>
        /// <param name="text">A string specifying the text the <see cref="Label">Label</see>
        /// will display.</param>
        /// <param name="foreground">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the foreground colour of the text of the <see cref="Label">Label</see>.</param>
        /// <param name="background">A <see cref="ConsoleColor">ConsoleColor</see> enumeration
        /// describing the background colour of the of the <see cref="Label">Label</see>.</param>
        public      Label
                    (string         name,
                     Point          location,
                     Int32          length,
                     string         text,
                     ConsoleColor   foreground,
                     ConsoleColor   background) : base(name, location, length, text, foreground, background) { }
        #endregion

        #region Private Methods
        /// <summary>
        /// A method to set the default foreground colour properties.
        /// </summary>
        private	void			InitializeColours() {
            this.Foreground = _DEFAULT_FOREGROUND;
            this.Background = _DEFAULT_BACKGROUND;
        }
        #endregion
	
        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Label">Label</see>
        /// object.</summary>
        /// <param name="writer">The stream to which this object will be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);
            base.WriteXml(writer);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Reads Xml when the <see cref="Label">Label</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            // Nothing to do but ship the work off to the base class.
            base.ReadXml(reader);
        }
        #endregion
   }
}