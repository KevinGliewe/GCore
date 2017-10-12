using System;
using System.Xml;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// Representation of a vertical or horizontal line on the console.
    /// </summary>
    public class Line	: System.Xml.Serialization.IXmlSerializable, 
                          System.IDisposable {
        #region Public Enums
        /// <summary>
        /// An enumeration to describe the direction of the line to be drawn, from
        /// the origin.
        /// </summary>
        public   enum     LineOrientation {
            /// <summary>
            /// Draw the line down from the origin.
            /// </summary>
            Vertical    = 0,

            /// <summary>
            /// Draw the line to the right of the origin.
            /// </summary>
            Horizontal  = 1
        }
        #endregion

        #region Private Members
        private     LineOrientation   _orientation      = Line.LineOrientation.Horizontal;
        private     ConsoleColor      _colour;
        private     Point             _location         = new Point();
        private     Int32             _length           = 0;
        #endregion

        #region ctors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Line() {}

        /// <summary>
        /// Constructor used to initialize the <see cref="Line">Line</see> object with
        /// initial values for orientation, colour and location.
        /// </summary>
        /// <param name="orientation">Whether the line runs vertically or horizontally.</param>
        /// <param name="colour">The colour of the line.</param>
        /// <param name="location">The starting x and y coordinates of the line origin.</param>
        /// <param name="length">The length of the line, either horizontally or 
        /// vertically, from the location.</param>
        public      Line
                   (LineOrientation orientation, 
                    ConsoleColor    colour, 
                    Point           location, 
                    Int32           length) : this() {
            _orientation = orientation;
            _colour = colour;
            _location = location;
            _length = length;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// A property indicating whether the line runs horizontally or vertically.
        /// </summary>
        public   LineOrientation   Orientation {
            get {return _orientation;}
            set {_orientation = value;}
        }

        /// <summary>
        /// The colour attributes of the line to draw.
        /// </summary>
        public   ConsoleColor      Colour {
            get {return _colour;}
            set {_colour = value;}
        }

        /// <summary>
        /// A <see cref="Point">Point</see> object representing the X and
        /// Y coordinates of the start of the line.
        /// </summary>
        public   Point             Location {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// An <see cref="Int32">Int32</see> describing the length of the line.
        /// </summary>
        public   Int32             Length {
            get {return _length;}
            set {_length = value;}
        }
        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Line">Line</see>
        /// object.</summary>
        /// <param name="writer">The stream to which this object will
        /// be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);
            writer.WriteAttributeString(string.Empty, "Orientation", string.Empty, _orientation.ToString());
            writer.WriteAttributeString(string.Empty, "Length", string.Empty, _length.ToString());
            writer.WriteAttributeString(string.Empty, "Colour", string.Empty, _colour.ToString());

            ((IXmlSerializable)_location).WriteXml(writer);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        /// <returns></returns>
        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema() {
            // TODO:  Add Line.GetSchema implementation
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="Line">Line</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            _orientation = (LineOrientation)Enum.Parse(_orientation.GetType(), 
                                                       reader.GetAttribute("Orientation"));
            _length = Int32.Parse(reader.GetAttribute("Length"));

            string colour = reader.GetAttribute("Colour");
            if (colour != null && colour.Length > 0)
                _colour = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colour);

            reader.Read();

            if (reader.Name == "Location") 
                ((IXmlSerializable)_location).ReadXml(reader);
            else
                throw new InvalidOperationException("<Location> node missing from <Line> node.");
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="Line">Line</see>
        /// object.
        /// </summary>
        void IDisposable.Dispose() {
            ((IDisposable)_location).Dispose();

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
