using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GCore.Yaml.Config;
using Xunit;

namespace GCore.Test.Yaml.Config
{
    public class ConfigTest
    {
        [Fact]
        public void Test1() {
            string result;
            using(Stream stream = typeof(TestClass).Assembly.GetManifestResourceStream("GCore.Test.Yaml.Config.default.yaml"))
            using(StreamReader reader = new StreamReader(stream)) {
                result = reader.ReadToEnd();
            }
            YamlConfig conf = new YamlConfig(new FileInfo("Test.yaml"), result);

            TestClass c = conf.GetMappingObject<TestClass>("Test.ASDF");

            Assert.Equal(c._double, 1.1);
            Assert.Equal(c._int, 42);
            Assert.Equal(c._bool, true);
            Assert.Equal(c._string, "Hello World");
            Assert.Equal(c._class2._stringlist[0], "Hello");
            Assert.Equal(c._class2._stringlist[1], "World");
            Assert.Equal(c.ConfigURL, "Test.ASDF");
            Assert.Equal(c._class2.ConfigURL, "Test.ASDF.class2");
        }
    }
}
