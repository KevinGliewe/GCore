using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Yaml;
using System.Reflection;
using GCore.Logging;

namespace GCore.Yaml.Config {
    public static class Extension {
        private static string typeKey = "type";

        public static Type GetNodeType(this DataItem this_) {
            if (this_ is Mapping) {
                Mapping objectMap = (Mapping)this_;
                string typeName = null;
                foreach (MappingEntry mn in objectMap.Enties) {
                    if (mn.Key.GetTextOrDefault("") == Extension.typeKey) {
                        typeName = mn.Value.GetTextOrDefault(null);
                        if (typeName != null) break;
                    }
                }
                if (typeName != null) {
                    return Type.GetType(typeName);
                }
            }

            return null;
        }

        public static object ToType(this DataItem this_, Type type, YamlConfig conf) {
            if(type == this_.GetNodeType()) {
                return conf.GetMappingObject(this_);
            }
            if(!(this_ is Sequence)) {
                return TConverter.ChangeType(type, this_.GetTextOrDefault(""));
            }
            if(!typeof(Array).Equals(type) && !typeof(Array).IsSubclassOf(type) &&
                !typeof(Object[]).Equals(type) && !typeof(Object[]).IsSubclassOf(type)) {

                Log.Warn(string.Format("[{0}] Cant parse Sequence to {1}", conf.GetURL(this_), type.Name), new object[0]);
                return null;
            }
            List<object> list = new List<object>();
            foreach(DataItem this_2 in (this_ as Sequence).Enties) {
                list.Add(this_2.ToType(conf));
            }
            return list.ToArray();
        }

        public static object ToType(this DataItem this_, YamlConfig conf) {
            Type type = this_.GetNodeType();
            if(type != null) {
                return conf.GetMappingObject(this_);
            }
            if(!(this_ is Sequence)) {
                string textOrDefault = this_.GetTextOrDefault("");
                type = StringTypeResolver.Resolve(textOrDefault);
                return TConverter.ChangeType(type, textOrDefault);
            }
            if(!typeof(Array).Equals(type) && !typeof(Array).IsSubclassOf(type) &&
                !typeof(Object[]).Equals(type) && !typeof(Object[]).IsSubclassOf(type)) {

                Log.Warn(string.Format("[{0}] Cant parse Sequence to {1}", conf.GetURL(this_), type.Name), new object[0]);
                return null;
            }
            List<object> list = new List<object>();
            foreach(DataItem this_2 in (this_ as Sequence).Enties) {
                list.Add(this_2.ToType(conf));
            }
            return list.ToArray();
        }

        public static string GetTextOrDefault(this DataItem this_, string def) {
            if(this_ is Scalar)
                return (this_ as Scalar).Text;
            return def;
        }

        public static bool EqualsContent(this DataItem this_, DataItem other) {
            if (this_.GetType() != other.GetType())
                return false;
            //return this_.GetContent() == other.GetContent();
            return this_.ToString() == other.ToString();
        }

        public static bool ContainsItem(this IEnumerable<DataItem> this_, DataItem item) {
            foreach (DataItem iitem in this_)
                if (iitem.EqualsContent(item))
                    return true;
            return false;
        }

        public static MappingEntry GetEntryByKey(this Mapping this_, string key) {
            foreach (MappingEntry entry in this_.Enties)
                if (entry.Key.GetTextOrDefault("") == key)
                    return entry;
            return null;
        }

        public static DataItem GetDataItemFromPath(this DataItem this_, IList<string> keys) {
            if (keys.Count == 0)
                return this_;
            if (!(this_ is Mapping))
                return null;
            MappingEntry entry = (this_ as Mapping).GetEntryByKey(keys[0]);
            if (entry == null)
                return null;
            keys.RemoveAt(0);
            return entry.Value.GetDataItemFromPath(keys);
        }

        public static string GetAnchor(this DataItem this_) {
            if (this_.Property == null)
                return null;
            return this_.Property.Anchor;
        }

        public static bool IsIMappingObject(this Type this_) {
            return typeof(IMappingObject).IsAssignableFrom(this_);
        }

        public static void PopulateMapping(this IMappingObject this_, MappingWrapper mapping) {
            Type type = this_.GetType();
            foreach(KeyValuePair<DataItem, DataItem> keyValuePair in mapping) {
                string text = "_" + keyValuePair.Key.GetTextOrDefault("").ToLower();
                if(!(text == "_type")) {
                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    FieldInfo fieldInfo = null;
                    foreach(FieldInfo fieldInfo2 in fields) {
                        if(fieldInfo2.Name.ToLower() == text) {
                            fieldInfo = fieldInfo2;
                            break;
                        }
                    }
                    if(fieldInfo == null) {
                        Log.Error(string.Format("[{0}] Member could not be found '{1}.{2}'", mapping.GetURL(), type.Name, text), new object[0]);
                    } else {
                        try {
                            object value = keyValuePair.Value.ToType(fieldInfo.FieldType, mapping.Config);
                            fieldInfo.SetValue(this_, value);
                        } catch(Exception excaption) {
                            Log.Exception(string.Format("[{0}] Exception while populateing member '{1}.{2}'", mapping.GetURL(), type.Name, text), excaption, new object[0]);
                        }
                    }
                }
            }
        }
    }
}
