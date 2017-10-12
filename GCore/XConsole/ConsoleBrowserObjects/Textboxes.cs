using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// A collection class of <see cref="Textbox">Textbox</see> objects.
    /// </summary>
    public class Textboxes : IEnumerable, 
                             IXmlSerializable,
                             IDisposable {
        private List<Textbox>   _textboxes  = new List<Textbox>();
        private Textbox         _focusField = null;

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="Textboxes">Textboxes</see>
        /// class.
        /// </summary>
        public   Textboxes() {}
        #endregion

        #region Public Properties
        /// <summary>
        /// Property to return the number of Textbox objects contained in
        /// the collection.
        /// </summary>
        public Int32 Count { get { return _textboxes.Count; } }
        #endregion

        #region Internal Methods
        internal void Rendered() {
            foreach (StdConsoleObject sco in _textboxes)
                sco.Rendered();
        }
        #endregion

        #region Internal Properties
        internal Textbox FocusField {
            get { return _focusField; }
            set {
                if (_focusField != value) {
                    _focusField = value;

                    foreach (Textbox t in _textboxes)
                        t.Focus = (t == _focusField);
                } 
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Appends a <see cref="Textbox">Textbox</see> to the list.
        /// </summary>
        /// <param name="t">The <see cref="Textbox">Textbox</see> to add.</param>
        public   void     Add(Textbox t) {
            _textboxes.Add(t);
        }

        /// <summary>
        /// Resets the string value of the text property of the textboxes in the
        /// collection to zero length strings.
        /// </summary>
        public   void     ClearValues() {
            foreach (Textbox t in _textboxes)
                t.Text = string.Empty;
        }
        #endregion
		
        #region Accessors
        /// <summary>
        /// Accessor to return a <see cref="Textbox">Textbox</see> based on
        /// the name of the Textbox.
        /// </summary>
        public	Textbox	this[string    name] {
            get {
                foreach (Textbox cTextbox in _textboxes)
                    if (cTextbox.Name == name)
                        return cTextbox;

                return null;
            }
        }

        /// <summary>
        /// Accessor to return a <see cref="Textbox">Textbox</see> based on
        /// its index in the collection.
        /// </summary>
        public	Textbox  this[Int32     index] {
            get {
                return _textboxes[index];
            }
        }
        #endregion
		
        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Textboxes">Textboxes</see>
        /// object.</summary>
        /// <param name="writer">The stream to which this object will be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);

            foreach (IXmlSerializable tb in _textboxes)
                tb.WriteXml(writer);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        /// <returns></returns>
        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema() {
            // TODO:  Add Textboxes.GetSchema implementation
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="Textboxes">Textboxes</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            if (!reader.IsEmptyElement) {
                while (reader.Read()) {
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement) {
                        reader.Read();
                        break;
                    }

                    Textbox textbox = new Textbox();

                    ((IXmlSerializable)textbox).ReadXml(reader);
                    _textboxes.Add(textbox);

                    reader.Read();
                }
            } else
                reader.Read();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="Textboxes">Textboxes</see>
        /// object.
        /// </summary>
        void IDisposable.Dispose() {
            foreach (IDisposable textbox in _textboxes)
                textbox.Dispose();

            _textboxes.Clear();

            GC.SuppressFinalize(this);
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Interface implementaion method to allow foreach iteration over
        /// the textboxes contained within this object.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator() {
            return new TextboxEnumerator(_textboxes);
        }
        #endregion

        /// <summary>
        /// Class used to provide an <see cref="IEnumerator">IEnumerator</see> object to
        /// whoever wants to iterate over the collection.
        /// </summary>
        private class TextboxEnumerator : System.Collections.IEnumerator {
            #region Private Members
            private List<Textbox> _textboxes    = null;
            private Int32         _pointer      = -1;
            #endregion

            #region ctors
            /// <summary>
            /// Constructor used to initialize a new enumerator for the collection
            /// of <see cref="Textbox">Textbox</see> objects.
            /// </summary>
            /// <param name="textboxes"></param>
            public TextboxEnumerator(List<Textbox> textboxes) {
                _textboxes = textboxes;
            }
            #endregion

            #region IEnumerator Members
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public object Current {
                get { return _textboxes[_pointer]; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext() {
                return ++_pointer != _textboxes.Count;
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