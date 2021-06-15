using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace GCore.XConsole.ConsoleBrowserObjects {
    /// <summary>
    /// A collection class of <see cref="Label">Label</see> objects.
    /// </summary>
    public class Labels : IEnumerable,
                          IXmlSerializable,
                          IDisposable {
        #region Private Members
        private List<Label> _labels = new List<Label>();    // The container of the Label objects.
        #endregion

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="Labels">Labels</see> collection object.
        /// </summary>
        public Labels() {}
        #endregion

        #region Public Properties
        /// <summary>
        /// Property to return the number of <see cref="Label">Label</see> objects currently
        /// in the collection.
        /// </summary>
        public Int32 Count { get { return _labels.Count; } }
        #endregion

        #region Internal Methods
        internal void Rendered() {
            foreach (StdConsoleObject sco in _labels)
                sco.Rendered();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a <see cref="Label">Label</see> object to the 
        /// <see cref="Labels">Labels</see> collection.</summary>
        /// <param name="l"><see cref="Label">Label</see> object to add to the collection.</param>
        public   void     Add(Label l) {
            _labels.Add(l);
        }
        #endregion
		
        #region Accessors
        /// <summary>
        /// Get a <see cref="Label">Label</see> in the collection based on it's name.
        /// </summary>
        public   Label	this[string    name] {
            get {
                foreach (Label cLabel in _labels)
                    if (cLabel.Name == name)
                        return cLabel;

                return null;
            }
        }

        /// <summary>
        /// Get a <see cref="Label">Label</see> in the collection based on it's index.
        /// </summary>
        public Label	this[Int32		index] {
            get {
                return _labels[index];
            }
        }
        #endregion
	
        #region IXmlSerializable Members
        /// <summary>
        /// Writes Xml articulating the current state of the <see cref="Labels">Labels</see>
        /// object.</summary>
        /// <param name="writer">The stream to which this object will be serialized.</param>
        void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            writer.WriteStartElement(this.GetType().Name);

            // Persist the state of the labels in the collection.
            foreach (IXmlSerializable l in _labels)
                l.WriteXml(writer);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Method that returns schema information.  Not yet implemented.
        /// </summary>
        System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema() {
            // TODO:  Add Labels.GetSchema implementation
            return null;
        }

        /// <summary>
        /// Reads Xml when the <see cref="Labels">Labels</see> is to be deserialized 
        /// from a stream.</summary>
        /// <param name="reader">The stream from which the object will be deserialized.</param>
        void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            // Advance until the first Label node is recognized.
            if (!reader.IsEmptyElement) {
                while (reader.Read()) {
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement) {
                        reader.Read();
                        break;
                    }

                    // Found a Label node. Deserialize it into a new Label object.
                    GCore.XConsole.ConsoleBrowserObjects.Label      label     = new Label();

                    ((IXmlSerializable)label).ReadXml(reader);
                    _labels.Add(label);

                    // Move to the next node.
                    reader.Read();
                }
            } else  // The reader is empty.  Advance to the next node in anticipation
                    // of finding a Textboxes node.
                reader.Read();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by this <see cref="Labels">Labels</see>
        /// object.
        /// </summary>
        public void Dispose() {
            foreach(IDisposable label in _labels)
                label.Dispose();

            _labels.Clear();

            GC.SuppressFinalize(this);
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that can iterate through the collection of
        /// <see cref="Label">Label</see> objects.
        /// </summary>
        /// <returns>An <see cref="IEnumerator">IEnumerator</see> that can be used 
        /// to iterate through the collection.</returns>
        public System.Collections.IEnumerator GetEnumerator() {
            return new LabelEnumerator(_labels);
        }
        #endregion

        /// <summary>
        /// Class used to provide an <see cref="IEnumerator">IEnumerator</see> object to
        /// whoever wants to iterate over the collection.
        /// </summary>
        private class LabelEnumerator : IEnumerator {
            #region Private Members
            private     List<Label> _labels = null;
            private     Int32       _pointer = -1;
            #endregion

            #region ctors
            /// <summary>
            /// Constructor used to initialize a new enumerator for the collection
            /// of <see cref="Label">Label</see> objects.
            /// </summary>
            /// <param name="labels"></param>
            public LabelEnumerator(List<Label> labels) {
                _labels = labels;
            }
            #endregion

            #region IEnumerator Members
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public object Current {
                get { return _labels[_pointer]; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext() {
                return ++_pointer != _labels.Count;
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