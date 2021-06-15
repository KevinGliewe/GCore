using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCore.Yaml;

namespace GCore.Yaml.Config {
    public interface IMappingObject {
        MappingWrapper Mapping { get; }
        void ReadMapping(MappingWrapper mapping, YamlConfig config);
        string ConfigURL { get; }
        void OnShutDown(YamlConfig conf);
    }
}
