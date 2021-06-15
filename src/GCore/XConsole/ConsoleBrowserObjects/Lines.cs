using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GCore.XConsole.ConsoleBrowserObjects {
	/// <summary>
	/// Collection class of <see cref="Line">Line</see> objects.
	/// </summary>
	public class Lines   :  IEnumerable,
                            IXmlSerializable,
                            IDisposable {

        #region Private Members
        private List<Line> _lines = new List<Line>();   // The container of Line objects.
        #endregion

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="Lines">Lines</see> collection object.
        /// </summary>
        public Lines()	{}
        #endregion

        #region Public Properties
        /// <summary>
        /// Property to return the number of <see cref="Line">Line</see> objects currently
        /// in the collection.
        /// </summary>
        public Int32 Count { get { return _lines.Count; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a <see cref="Line">Line</see> object to the 
        /// <see cref="Lines">Lines</see> collection.</summary>
        /// <param name="line"><see cref="Line">Line</see> object to add to the collection.</param>
        public   void     Add(Line line) {
            _lines.Add(line);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Get a <see cref="Line">Line</see> in the collection based on it's index.
        /// </summary>
        public Line    this[Int32     index] {
            get { return _lines[index]; }
        }
        #endregion

        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Lines">Lines</see>
        /// object.</summary>
        /// <param name="writer">The stream to which this object will
        /// be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);

            foreach (IXmlSerializable line in _lines)
                line.WriteXml(writer);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema() {
            // TODO:  Add Lines.GetSchema implementation
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="Lines">Lines</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            if (!reader.IsEmptyElement) {
                while (reader.Read()) {
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement) {
                        reader.Read();
                        break;
                    }

                    GCore.XConsole.ConsoleBrowserObjects.Line    line   = new Line();

                    ((IXmlSerializable)line).ReadXml(reader);
                    _lines.Add(line);

                    reader.Read();
                }
            } else
                reader.Read();
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="Lines">Lines</see>
        /// object.
        /// </summary>
        void IDisposable.Dispose() {
            foreach(IDisposable line in _lines)
                line.Dispose();

            _lines.Clear();

            GC.SuppressFinalize(this);
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that can iterate through the collection of
        /// <see cref="Line">Line</see> objects..
        /// </summary>
        /// <returns>An <see cref="IEnumerator">IEnumerator</see> that can be used 
        /// to iterate through the collection.</returns>
        public IEnumerator GetEnumerator() {
            return new LineEnumerator(_lines);
        }
        #endregion

        /// <summary>
        /// Class used to provide an <see cref="IEnumerator">IEnumerator</see> object to
        /// whoever wants to iterated over the collection.
        /// </summary>
        private class LineEnumerator : System.Collections.IEnumerator {
            #region Private Members
            private     List<Line>  _lines = null;
            private     Int32       _pointer = -1;
            #endregion

            /// <summary>
            /// Constructor used to initialize a new enumerator for the collection
            /// of <see cref="Line">Line</see> objects.
            /// </summary>
            /// <param name="lines"></param>
            public LineEnumerator(List<Line> lines) {
                _lines = lines;
            }

            #region IEnumerator Members
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public object Current {
                get { return _lines[_pointer]; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext() {
                return ++_pointer != _lines.Count;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first 
            /// element in the collection.
            /// </summary>
            public void Reset() {
                _pointer = -1;
            }
            #endregion
        }
    }
}
