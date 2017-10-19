using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using GCore.Yaml;
using GCore.Extensions.FileInfoEx;
using GCore.Logging;


namespace GCore.Yaml.Config {
    public class YamlConfig {

        #region Members
        private DataItem _rootNode;
        private FileInfo _configFile;
        private Dictionary<string, IMappingObject> _refObjects = new Dictionary<string, IMappingObject>();

        private bool _active = true;

        private YamlStream _yamlStream;
        #endregion

        #region Events
        public event Action<YamlConfig> ShutDown;
        #endregion

        #region Initialization
        public YamlConfig(FileInfo config, String def = null) {

            if (config.Exists) {
                YamlStream conf = Helper.Parse(config);
                this._rootNode = conf.Documents[0].Root;
                _yamlStream = conf;
            } else {
                if(def != null)
                    config.SetString(def);
            }

            if (def != null) {
                YamlStream conf = Helper.Parse(def);
                this._rootNode = this.MergeNodes(this._rootNode, conf.Documents[0].Root);
            }

            if(this._rootNode == null)
                this._rootNode = new Mapping();

            
        }
    
        #endregion

        #region Finalization
        ~YamlConfig() {

        }
        #endregion

        #region Interface
        public void Shutdown() {
            if (!this._active)
                return;
            this._active = false;
            foreach (IMappingObject obj in this._refObjects.Values)
                obj.OnShutDown(this);
            if (this.ShutDown != null)
                this.ShutDown(this);
        }

        public string GetURL(DataItem item) {
            List<string> url = new List<string>();
            this._getURL(item, this._rootNode, url);
            return string.Join("", url).TrimStart('.');
        }

        public DataItem GetDataItem(string key) {
            List<string> keys = new List<string>(key.Split('.'));
            return this.GetRootNode().GetDataItemFromPath(keys);
        }

        public T GetMappingObject<T>(string key) where T: IMappingObject {
            DataItem item = this.GetDataItem(key);
            if (item == null)
                return default(T);
            return (T)this.GetMappingObject(item);
        }

        public IMappingObject GetMappingObject(DataItem item) {
            if (!(item is Mapping)) {
                Log.Warn(string.Format("Item ({0}): Should be a \"Mapping\", but is a \"{1}\"",this.GetURL(item), item.GetType().Name));
                return null; 
            }
            Type type = item.GetNodeType();
            if (type == null) {
                Log.Warn(string.Format("Item ({0}): Type '{1}' not found",
                                        this.GetURL(item),
                                        new MappingWrapper((item as Mapping), this).GetOrDef("type", "NO TYPE DEFINED")
                                        ));
                return null; 
            }
            if (!type.IsIMappingObject()) {
                Log.Warn(string.Format("Item ({0}): Type '{1}' is not castable to 'IMappingObject'", this.GetURL(item), type.Name));
                return null; 
            }
            IMappingObject obj = null;
            if (item.GetAnchor() != null) {
                if(this._refObjects.ContainsKey(item.GetAnchor()))
                    obj = this._refObjects[item.GetAnchor()];
            }
            if(obj == null) {
                obj = (IMappingObject)Activator.CreateInstance(type);
                obj.ReadMapping(new MappingWrapper(item as Mapping, this), this);
                if (item.GetAnchor() != null && !this._refObjects.ContainsKey(item.GetAnchor()))
                    this._refObjects.Add(item.GetAnchor(), obj);
            }
            return obj;
        }
        #endregion

        #region Interface(IConfig)
        public DataItem GetRootNode() {
            return this._rootNode;
        }
        public FileInfo GetConfigFile() {
            return this._configFile;
        }
        #endregion

        #region Tools
        private DataItem MergeNodes(DataItem prim, DataItem sec) {
            if (prim == null)
                return sec;
            if (sec == null)
                return prim;
            if (prim.GetType() != sec.GetType())
                return prim;
            //if (prim.EqualsContent(sec))
            //    return prim;

            Type dataType = prim.GetType();

            if (dataType == typeof(Mapping)) {
                MappingWrapper mPri = new MappingWrapper(prim as Mapping, this);
                MappingWrapper mSec = new MappingWrapper(sec as Mapping, this);

                foreach (MappingEntry eSec in mSec.Mapping.Enties) {
                    MappingEntry ePri = mPri.GetMappingNodeByKey(eSec.Key);
                    if (ePri == null) {
                        mPri.Add(eSec);
                    } else {
                        ePri.Value = this.MergeNodes(ePri.Value, eSec.Value);
                    }
                }
            } else if (dataType == typeof(Sequence)) {
                Sequence sPri = prim as Sequence;
                Sequence sSec = sec as Sequence;

                foreach (DataItem item in sSec.Enties) {
                    if (!sPri.Enties.ContainsItem(item))
                        sPri.Enties.Add(item);
                }
            }

            return prim;
        }

        private bool _getURL(DataItem searched, DataItem currentItem, List<string> url) {
            if (searched == currentItem)
                return true;

            if (currentItem is Sequence) {
                int n = 0;
                foreach (DataItem item in (currentItem as Sequence).Enties) {
                    if (this._getURL(searched, item, url)) {
                        url.Insert(0, "[" + n + "]");
                        return true;
                    }
                    n++;
                }
                return false;
            }

            if (currentItem is Mapping) {
                foreach (MappingEntry entry in (currentItem as Mapping).Enties) {
                    if (this._getURL(searched, entry.Key, url)) {
                        url.Insert(0, ".$");
                        return true;
                    }
                    if (this._getURL(searched, entry.Value, url)) {
                        url.Insert(0, "."+entry.Key.GetTextOrDefault("?$?"));
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        #endregion

        #region Browsable Properties
        public bool Active { get { return this._active; } }
        public YamlStream YamlStream { get { return _yamlStream; } }
        #endregion

        #region NonBrowsable Properties
        #endregion

        #region Static

        #region Static Public

        #endregion

        #region Static Private

        #endregion

        #endregion
    }
}