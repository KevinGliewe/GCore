using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GCore.Sys.Process {
    public static class Utils {
        public static string GetLastMethodName() {
            return new StackTrace().ToString().Split('\n')[2].Replace("\r", "").Trim();
        }
    }
}
