using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GCore.Yaml;
using GCore.Logging;


namespace GCore.Yaml.Config {
    public class MappingWrapper : IDictionary<DataItem, DataItem> {

        #region Members
        private Mapping _mapping;
        private YamlConfig _config;
        #endregion

        #region Events
        #endregion

        #region Initialization
        public MappingWrapper(Mapping mapping, YamlConfig config) {
            this._mapping = mapping;
            this._config = config;
        }
        #endregion

        #region Finalization
        ~MappingWrapper() {

        }
        #endregion

        #region Interface
        public void Add(MappingEntry node) {
            this._mapping.Enties.Add(node);
        }
        public MappingEntry GetMappingNodeByKey(DataItem key) {
            foreach (MappingEntry node in this._mapping.Enties)
                if (node.Key.EqualsContent(key))
                    return node;
            return null;
        }

        public DataItem Get(string key) {
            foreach (MappingEntry item in this._mapping.Enties)
                if (item.Key.GetTextOrDefault(null) == key)
                    return item.Value;
            return null;
        }

        public IMappingObject GetMO(string key) {
            DataItem item = this.Get(key);
            if (item == null)
                return null;
            return this._config.GetMappingObject(item);
        }

        public bool Get<T>(string key, out T val) {
            val = default(T);
            DataItem item = this.Get(key);
            if (item == null)
                return false;

            try {
                if (typeof(T).IsIMappingObject())
                    val = (T)this._config.GetMappingObject(item);
                else if (item is Scalar)
                    val = TConverter.ChangeType<T>((item as Scalar).Text);
                return val != null;
            } catch (Exception ex) { Log.Exception("Error while loading an IMappingObject", ex); }

            return false;
        }

        public bool Get<T>(string key, Action<T> callback) {
            T tmp;
            if (this.Get(key, out tmp)) {
                callback(tmp);
                return true;
            }
            return false;
        }

        public T GetOrDef<T>(string key, T def) {
            T tmp;
            if (this.Get<T>(key, out tmp))
                return tmp;
            return def;
        }

        public bool TryGet<T>(ref T var, string key) {
            T tmp;
            if (this.Get<T>(key, out tmp)) {
                var = tmp;
                return true;
            }
            return false;
        }


        public IEnumerable<string> GetKeys() {
            foreach (var entry in this) {
                string key = entry.Key.GetTextOrDefault(null);
                if(key != null)
                    yield return key;
            }
        }

        public string GetURL() {
            return this._config.GetURL(this._mapping);
        }
        #endregion

        #region Interface(IDictionary<DataItem, DataItem>)
        public void Add(DataItem key, DataItem value) {
            MappingEntry entry = new MappingEntry();
            entry.Key = key; entry.Value = value;
            this.Add(entry);
        }

        public bool ContainsKey(DataItem key) {
            foreach (MappingEntry node in this._mapping.Enties)
                if (node.Key.EqualsContent(key))
                    return true;
            return false;
        }

        public ICollection<DataItem> Keys {
            get {
                List<DataItem> keys = new List<DataItem>();
                foreach (MappingEntry node in this._mapping.Enties)
                    keys.Add(node.Key);
                return keys;
            }
        }

        public bool Remove(DataItem key) {
            lock (this._mapping) {
                MappingEntry mnode = this.GetMappingNodeByKey(key);
                if (mnode == null) return false;
                this._mapping.Enties.Remove(mnode);
                return true;
            }
        }

        public bool TryGetValue(DataItem key, out DataItem value) {
            value = null;
            lock (this._mapping) {
                MappingEntry mnode = this.GetMappingNodeByKey(key);
                if (mnode == null) return false;
                value = mnode.Value;
                return true;
            }
        }

        public ICollection<DataItem> Values {
            get {
                List<DataItem> values = new List<DataItem>();
                foreach (MappingEntry node in this._mapping.Enties)
                    values.Add(node.Value);
                return values;
            }
        }

        public DataItem this[DataItem key] {
            get {
                lock (this._mapping) {
                    MappingEntry mnode = this.GetMappingNodeByKey(key);
                    if (mnode == null)
                        return null;
                    return mnode.Value;
                }
            }
            set {
                lock (this._mapping) {
                    MappingEntry mnode = this.GetMappingNodeByKey(key);
                    if (mnode == null)
                        this.Add(key, value);
                    else
                        mnode.Value = value;
                }
            }
        }

        public void Add(KeyValuePair<DataItem, DataItem> item) {
            this.Add(item.Key, item.Value);
        }

        public void Clear() {
            this._mapping.Enties.Clear();
        }

        public bool Contains(KeyValuePair<DataItem, DataItem> item) {
            return this[item.Key].EqualsContent(item.Value);
        }

        public void CopyTo(KeyValuePair<DataItem, DataItem>[] array, int arrayIndex) {
            foreach (KeyValuePair<DataItem, DataItem> item in this) 
                array[arrayIndex++] = item;
        }

        public int Count {
            get { return this._mapping.Enties.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<DataItem, DataItem> item) {
            lock (this._mapping) {
                MappingEntry mnode = this.GetMappingNodeByKey(item.Key);
                if (mnode == null) return false;
                if (!mnode.Value.EqualsContent(item.Value)) return false;
                this._mapping.Enties.Remove(mnode);
                return true;
            }
        }

        public IEnumerator<KeyValuePair<DataItem, DataItem>> GetEnumerator() {
            foreach (MappingEntry node in this._mapping.Enties)
                yield return new KeyValuePair<DataItem, DataItem>(node.Key, node.Value);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            foreach (MappingEntry node in this._mapping.Enties)
                yield return new KeyValuePair<DataItem, DataItem>(node.Key, node.Value);
        }
        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        public Mapping Mapping { get { return this._mapping; } }
        public YamlConfig Config { get { return this._config; } }
        public String URL { get { return this._config.GetURL(this._mapping); } }
        #endregion

        #region NonBrowsable Properties
        #endregion


    }
}