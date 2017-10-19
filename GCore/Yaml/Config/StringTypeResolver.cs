using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GCore.Yaml.Config
{
    public static class StringTypeResolver {
        static StringTypeResolver() {
            StringTypeResolver.entrys.Add(new Regex("^[0-9]+$"), typeof(int));
            StringTypeResolver.entrys.Add(new Regex("^[0-9]+\\.[0-9]+$"), typeof(double));
            StringTypeResolver.entrys.Add(new Regex("^((T|t)rue|(F|f)alse)$"), typeof(bool));
        }


        public static Type Resolve(string str) {
            foreach(Regex regex in StringTypeResolver.entrys.Keys) {
                if(regex.IsMatch(str)) {
                    return StringTypeResolver.entrys[regex];
                }
            }
            return typeof(string);
        }

        private static Dictionary<Regex, Type> entrys = new Dictionary<Regex, Type>();
    }
}
