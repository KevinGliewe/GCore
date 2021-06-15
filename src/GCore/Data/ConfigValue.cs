using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Data;
using System.Text.RegularExpressions;

namespace GCore.Data {
    public class ConfigValue<T> {
        private INIFile iniFile = null;

        private T defaultValue = default(T);

        private string section = null;
        private string key = null;

        private Func<T, bool> validator = null;

        public ConfigValue(INIFile file, T defaultValue, string section, string key, Func<T, bool> validator = null) {
            this.iniFile = file;
            this.defaultValue = defaultValue;
            this.section = section;
            this.key = key;
            this.validator = validator;

            if (!this.iniFile.ContainsValue(this.section, this.key)) {
                this.SetValue(this.defaultValue);
            } else if (!this.Validate(this.Val)) {
                this.SetValue(this.defaultValue);
            }
        }

        #region Get/Set
        public T Val {
            get { return this.GetValue();  }
            set { this.SetValue(value); }
        }

        public T GetValue() {
            if (typeof(T) == typeof(int)) {
                return ConvertValue<T, int>(this.iniFile.GetValue(this.section, this.key, ConvertValue<int, T>(this.defaultValue)));
            } else if (typeof(T) == typeof(string)) {
                return ConvertValue<T, string>(this.iniFile.GetValue(this.section, this.key, ConvertValue<string, T>(this.defaultValue)));
            } else if (typeof(T) == typeof(byte[])) {
                return ConvertValue<T, byte[]>(this.iniFile.GetValue(this.section, this.key, ConvertValue<byte[], T>(this.defaultValue)));
            } else if (typeof(T) == typeof(double)) {
                return ConvertValue<T, double>(this.iniFile.GetValue(this.section, this.key, ConvertValue<double, T>(this.defaultValue)));
            } else if (typeof(T) == typeof(bool)) {
                return ConvertValue<T, bool>(this.iniFile.GetValue(this.section, this.key, ConvertValue<bool, T>(this.defaultValue)));
            }else {
                throw new Exception(string.Format("Generic Type '{1}' is not supported", typeof(T).Name));
            }
        }



        public void SetValue(T val) {
            if (!this.Validate(val)) {
                throw new Exception(string.Format("Value '{0}' is not valid", val));
            }

            if (typeof(T) == typeof(int)) {
                this.iniFile.SetValue(this.section, this.key, ConvertValue<int, T>(val));
            } else if (typeof(T) == typeof(string)) {
                this.iniFile.SetValue(this.section, this.key, ConvertValue<string, T>(val));
            } else if (typeof(T) == typeof(byte[])) {
                this.iniFile.SetValue(this.section, this.key, ConvertValue<byte[], T>(val));
            } else if (typeof(T) == typeof(double)) {
                this.iniFile.SetValue(this.section, this.key, ConvertValue<double, T>(val));
            } else if (typeof(T) == typeof(bool)) {
                this.iniFile.SetValue(this.section, this.key, ConvertValue<bool, T>(val));
            } else {
                throw new Exception(string.Format("Generic Type '{1}' is not supported", typeof(T).Name));
            }
            
        }
        #endregion

        #region Helper
        private static T1 ConvertValue<T1, T2>(T2 t) {
            return (T1)Convert.ChangeType(t, typeof(T1));
        }

        public bool Validate(T val) {
            if (this.validator != null) return this.validator(val);
            return true;
        }
        #endregion

        #region Validator
        public static bool Validator_IP(string v) {
            return new Regex("\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b").IsMatch(v);
        }

        public static bool Validator_Positive(int v) {
            return v >= 0;
        }

        public static bool Validator_Positive(double v) {
            return v >= 0.0;
        }
        #endregion
    }
}
