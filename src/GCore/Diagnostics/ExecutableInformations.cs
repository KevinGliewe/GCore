using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GCore.Diagnostics {
    public class ExecutableInformations {
        public static DateTime RetrieveLinkerTimestamp() {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            } finally {
                if (s != null) {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

        /// <summary>
        /// Need to do this: [assembly: AssemblyVersion("1.0.*")]
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static DateTime RetrieveVersionTimestamp(Assembly assembly) {
            System.Version MyVersion = assembly.GetName().Version;

            // MyVersion.Build = days after 2000-01-01
            // MyVersion.Revision*2 = seconds after 0-hour  (NEVER daylight saving time)
            DateTime MyTime = new DateTime(2000, 1, 1).AddDays(MyVersion.Build).AddSeconds(MyVersion.Revision * 2);
            return MyTime;
        }

        public static bool IsFullFramework => System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework");
    }
}
