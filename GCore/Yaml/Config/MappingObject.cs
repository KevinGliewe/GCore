using GCore.Extensions.ObjectEx;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GCore.Yaml.Config
{
    public class MappingObject : IMappingObject {
        ~MappingObject() {
        }

        protected virtual void ReadMapping(MappingWrapper mapping) {
        }

        public virtual string ToYaml(int indent = 0) {
            StringBuilder stringBuilder = new StringBuilder("\n");
            FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            string indet = MappingObject.GetIndet(indent);
            stringBuilder.AppendLine(string.Concat(new string[]
            {
                indet,
                "type: ",
                base.GetType().FullName,
                ", ",
                base.GetType().Assembly.GetName().Name
            }));
            foreach(FieldInfo fieldInfo in fields) {
                if(fieldInfo.Name.StartsWith("_")) {
                    string str = fieldInfo.Name.Substring(1);
                    object value = fieldInfo.GetValue(this);
                    string value2 = MappingObject.ObjectToYaml(value, indent + 1);
                    stringBuilder.Append(indet);
                    stringBuilder.Append(str + ":");
                    stringBuilder.AppendLine(value2);
                }
            }
            return stringBuilder.ToString();
        }

        public virtual MappingWrapper Mapping {
            get {
                return this._mapping;
            }
        }

        public void ReadMapping(MappingWrapper mapping, YamlConfig config) {
            this._mapping = mapping;
            this.PopulateMapping(mapping);
            this.ReadMapping(mapping);
        }

        public virtual string ConfigURL {
            get {
                if(this.Mapping != null) {
                    return this.Mapping.GetURL();
                }
                return "?NOCONFIG?";
            }
        }

        public virtual void OnShutDown(YamlConfig conf) {
            throw new NotImplementedException();
        }

        public static string GetIndet(int indent) {
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < indent; i++) {
                stringBuilder.Append("    ");
            }
            return stringBuilder.ToString();
        }

        public static string ObjectToYaml(object ovalue, int indent) {
            if(ovalue.IsCastableTo(typeof(MappingObject))) {
                return (ovalue as MappingObject).ToYaml(indent + 1);
            }
            if(ovalue.IsCastableTo(typeof(Array))) {
                return MappingObject.ArrayToYaml(ovalue as Array, indent + 1);
            }
            string result;
            using(new Helper.InvariantCulture()) {
                result = " " + ovalue.ToString();
            }
            return result;
        }

        public static string ArrayToYaml(Array list, int indent = 0) {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\n");
            foreach(object ovalue in list) {
                stringBuilder.Append(MappingObject.GetIndet(indent) + "-");
                stringBuilder.AppendLine(MappingObject.ObjectToYaml(ovalue, indent + 1));
            }
            return stringBuilder.ToString();
        }
       
        public const string INDENT = "    ";
        
        private MappingWrapper _mapping;
    }
}
