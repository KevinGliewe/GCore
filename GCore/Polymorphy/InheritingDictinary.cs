using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 19.11.2013 09:29:10
//Datei  : InheritingDictinary.cs


namespace GCore.Data {
    /// <summary>
    /// Ein Dictionary, das von einem anderen Dictionary erben kann
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class InheritingDictinary<TKey, TValue> : IDictionary<TKey, TValue> {

        #region Members
        //Eigentliches Dictionary
        private IDictionary<TKey, TValue> innerDict;

        //Das Dictionary von dem geerbt wird
        private IDictionary<TKey, TValue> inheritedDict;
        #endregion

        #region Events
        #endregion

        #region Initialization
        public InheritingDictinary() : this(new Dictionary<TKey, TValue>()) { }

        public InheritingDictinary(IDictionary<TKey, TValue> dict) {
            this.innerDict = dict;
        }
        #endregion

        #region Finalization
        ~InheritingDictinary() {

        }
        #endregion

        #region Interface
        /// <summary>
        /// Gibt die Vererbungstiefe des Schlüssels zurück
        /// </summary>
        /// <param name="key">Schlüssel für das Dictionary</param>
        /// <returns>Vererbungstiefe</returns>
        public int DepthOfKey(TKey key) {
            if (this.ContainsKey(key) == false)
                throw new KeyNotFoundException();

            //Wenn der schlüssel im aktuellen Dictionary enthalten ist, ist die Tiefe 0
            if (this.innerDict.ContainsKey(key))
                return 0;
            if (this.inheritedDict != null)
                if (this.inheritedDict is InheritingDictinary<TKey, TValue>) {
                    return (this.inheritedDict as InheritingDictinary<TKey, TValue>).DepthOfKey(key) + 1;
                } else {
                    //Wenn es kein erbendes Dictionary ist
                    return 1;
                }
            return -1;
        }
        #endregion

        #region Interface(IDictionary<TKey, TValue>)
        public void Add(TKey key, TValue value) {
            this.innerDict.Add(key, value);
        }

        public bool ContainsKey(TKey key) {
            if (this.innerDict.ContainsKey(key))
                return true;
            if (this.inheritedDict != null)
                return this.inheritedDict.ContainsKey(key);
            return false;
        }

        public ICollection<TKey> Keys {
            get {
                List<TKey> keys = this.innerDict.Keys.ToList();

                if (this.inheritedDict != null) {
                    keys.AddRange(this.inheritedDict.Keys);
                    keys = keys.Distinct().ToList();
                }

                return keys;
            }
        }

        public bool Remove(TKey key) {
            return this.innerDict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            if (this.innerDict.TryGetValue(key, out value))
                return true;

            if (this.inheritedDict != null)
                return this.inheritedDict.TryGetValue(key, out value);

            return false;
        }

        public ICollection<TValue> Values {
            get {
                List<TValue> values = this.innerDict.Values.ToList();

                if (this.inheritedDict != null) {
                    values.AddRange(this.inheritedDict.Values);
                }

                return values;
            }
        }

        public TValue this[TKey key] {
            get {
                if (this.innerDict.ContainsKey(key))
                    return this.innerDict[key];

                if (this.inheritedDict != null && this.inheritedDict.ContainsKey(key))
                    return this.inheritedDict[key];

                throw new KeyNotFoundException();
            }
            set {
                this.innerDict[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            this.innerDict.Add(item);
        }

        public void Clear() {
            this.innerDict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            if (this.innerDict.Contains(item))
                return true;

            if (this.inheritedDict != null)
                return this.inheritedDict.Contains(item);

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            this.innerDict.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return this.Keys.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return this.innerDict.Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {

            foreach (TKey key in this.Keys)
                yield return new KeyValuePair<TKey, TValue>(key, this[key]);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        public IDictionary<TKey, TValue> InnerDictionary {
            get { return this.innerDict; }
        }

        /// <summary>
        /// das Dictionary von dem geerbt wird
        /// </summary>
        public IDictionary<TKey, TValue> InheritedDictionary {
            get { return this.inheritedDict; }
            set {
                this.inheritedDict = value;
            }
        }

        /// <summary>
        /// Die Vererbungstiefe
        /// </summary>
        public int Depth {
            get {
                if (this.inheritedDict != null)
                    if (this.inheritedDict is InheritingDictinary<TKey, TValue>)
                        return (this.inheritedDict as InheritingDictinary<TKey, TValue>).Depth + 1;
                    else
                        return 1;
                return 0;
            }
        }
        #endregion

        #region NonBrowsable Properties
        #endregion

    }
}