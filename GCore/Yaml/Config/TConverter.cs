using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace GCore.Yaml.Config {
    public static class TConverter {
        public static CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-US");

        public static T ChangeType<T>(object value) {
            return (T)ChangeType(typeof(T), value);
        }
        public static object ChangeType(Type t, object value) {
            using (new GCore.Globalisation.Culture()) {
                TypeConverter tc = TypeDescriptor.GetConverter(t);
                object tmp = tc.ConvertFrom(value);
                return tmp;
            }
        }
        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter {

            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }
}
