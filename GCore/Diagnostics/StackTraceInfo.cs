using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using GCore.Extensions.StringEx;
using GCore.Extensions.StringEx.Inject;
using System.Collections;

namespace GCore.Diagnostics {
    public class StackTraceInfo : IFormattable {
        private string[] stackTrace;
        private FunctionInfo[] functionInfos;

        private int baseoffset;

        private static char[] intFilter = new char[] {
            '0', '1', '2', '3', '4', '5', '6', '7', '8','9', '-'
        };

        public StackTraceInfo(int baseoffset = 0) {
            this.baseoffset = baseoffset;

            this.stackTrace = (new StackTrace()).ToString().Replace("\r", "").Split('\n');
            this.functionInfos = new FunctionInfo[this.stackTrace.Length];
        }

        public FunctionInfo GetFunctionInfoFromOffset(int offset) {
            offset += baseoffset + 1;

            if (this.functionInfos[offset] == null)
                this.functionInfos[offset] = new FunctionInfo(this.stackTrace[offset]);

            return this.functionInfos[offset];
        }

        public string ToString(string format, IFormatProvider provider) {
            int offset = 0;

            string offsetString = format.Filter(
              intFilter,
              true);

            int.TryParse(offsetString, out offset);

            return this.GetFunctionInfoFromOffset(offset).ToString(!format.Contains("n"), !format.Contains("p"));
        }

        public override string ToString() {
            return this.GetFunctionInfoFromOffset(0).ToString(true, true);
        }

        public int BaseOffset {
            get { return this.baseoffset; }
            set { this.baseoffset = value; }
        }

        public static string Inject(string text, int offset = 0, IDictionary dict = null) {
            return text.Inject(new Hashtable(dict) {
                {"stacktrace", new StackTraceInfo(offset + 1)}
            });
        }
    }
}
