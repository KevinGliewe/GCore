using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Yaml;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace GCore.Yaml.Config {
    public static class Helper {
        public static YamlStream Parse(FileInfo conf) {
            return Parse(conf.OpenText());
        }
        public static YamlStream Parse(StreamReader conf) {
            YamlStream tmp = Parse(conf.ReadToEnd());
            conf.Close();
            return tmp;
        }
        public static YamlStream Parse(String conf) {
            conf = Tab2Spaces(conf);
            bool tmp;
            YamlParser parser = new YamlParser();
            YamlStream stream = parser.ParseYamlStream(new TextInput(conf), out tmp);
            if (!tmp)
                throw new Exception(parser.GetEorrorMessages());
            return stream;
        }

        public static Type GetType(string name) {
            Type type = Type.GetType(name);
            if(type == null)
                type = AppDomain.CurrentDomain.GetAssemblies()
                                       .Select(a => new { a, a.GetTypes().First().Namespace })
                                       .Select(x => x.a.GetType(x.Namespace + "." + name))
                                       .FirstOrDefault(x => x != null);
            return type;
        }

        public static string Tab2Spaces(string text) {
            //return text;
            /*Regex regex = new Regex("^(\t|    )+");
            string[] lines = text.Replace("\r", "").Split('\n');
            for (int i = 0; i < lines.Length; i++ )
                lines[i] = regex.Replace(lines[i], "    ");
            return string.Join("\n", lines);*/
            return text.Replace("\t", "    ");
        }

        public class InvariantCulture : IDisposable {
            public InvariantCulture() {
                this.m_culture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            }

            public void Dispose() {
                Thread.CurrentThread.CurrentCulture = this.m_culture;
            }

            private CultureInfo m_culture;
        }
    }
}
