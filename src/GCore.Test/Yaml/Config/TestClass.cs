using GCore.Yaml.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace GCore.Test.Yaml.Config {
    public class TestClass : MappingObject {
        public double _double;
        public int _int;
        public bool _bool;
        public string _string;
        public TestClass2 _class2;
    }

    public class TestClass2 : MappingObject {
        public Object[] _stringlist;
    }
}
