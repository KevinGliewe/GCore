using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GCore.Extensions.ArrayEx;
using GCore.Extensions.ObjectEx;
using GCore.Extensions.StringEx;

namespace GCore.Data{
    public class DB {
        #region Static

        private static DB _instance;

        public static DB I {
            get {
                if (_instance == null) {
                    _instance = new DB(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + System.AppDomain.CurrentDomain.FriendlyName.Replace("vshost.","").Replace(".exe","") + ".bin");
                }
                return _instance;
            }
        }
        
        public static bool Init(string pathOfDB)
        {
        	if (_instance == null) {
                 _instance = new DB(pathOfDB);
                 return true;
            }
        	return false;
        }

        public static ByteArrayExtensions.DataFormatType DefaultFormatType = ByteArrayExtensions.DataFormatType.Binary;
        #endregion


        public ByteArrayExtensions.DataFormatType FormatType = ByteArrayExtensions.DataFormatType.Binary;

        private bool _editMode = false;

        public FileInfo dbFile;
        public Dictionary<object, object> dicDb = new Dictionary<object, object>();

        public DB(string file) {
            this.FormatType = DefaultFormatType;
            this.initDB(file);
        }

        public DB(string file, ByteArrayExtensions.DataFormatType format) {
            this.FormatType = format;
            this.initDB(file);
        }

        public Object this[object index] {
            get {
                if (dicDb.ContainsKey(index)) return dicDb[index];
                else return null;
            }
            set {
                if (dicDb.ContainsKey(index)) {
                    dicDb[index] = value;
                } else {
                    dicDb.Add(index, value);
                }
                if (!this._editMode)
                    _saveDB();
            }
        }

        public void DeleteRecord(Object key) {
            if(dicDb.ContainsKey(key)){
                dicDb.Remove(key);
                if (!this._editMode)
                    _saveDB();
            }
        }

        public bool ContainsKey(Object key) { return dicDb.ContainsKey(key); }

        public IEnumerable<Object> Glob(string pattern)
        {
            return from pair in this.dicDb where pair.Key is String where ((string)pair.Key).Like(pattern) select pair.Value;
        }
        
        public IEnumerable<T> Glob<T>(string pattern)
        {
        	return from pair in this.dicDb where pair.Key is String where ((string)pair.Key).Like(pattern) select (T)pair.Value;
        }

        public IEnumerable<KeyValuePair<Object, Object>> GlobPair(string pattern) {
            return from pair in this.dicDb where pair.Key is String where ((string)pair.Key).Like(pattern) select pair;
        }

        public Object GetOrDefault(Object key, Object defaultValue)
        {
            if(this.dicDb.ContainsKey(key))
                return this.dicDb[key];
            return defaultValue;
        }

        public void initDB(string dbfile) {
            dbFile = new FileInfo(dbfile);
            try {
                System.IO.FileStream fs = new System.IO.FileStream(dbFile.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                fs.Close();
                dicDb = (Dictionary<object, object>)data.DeSerialize(FormatType);
                if (dicDb == null)
                    dicDb = new Dictionary<object, object>();
            } catch (Exception ex) { }
        }
        public object[] GetKeys() {
            return dicDb.Keys.ToArray();
        }

        private void _saveDB() {
            dicDb.Serialize(FormatType).SaveToFile(dbFile.FullName);
        }

        public void BeginEdit() { this._editMode = true; }
        public void EndEdit() { this._editMode = false; _saveDB(); }
    }

}
