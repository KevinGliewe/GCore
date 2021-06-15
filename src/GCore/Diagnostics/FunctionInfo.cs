using System;
 
using System.Diagnostics;
using System.Text.RegularExpressions;
using GCore.Logging;
 
namespace GCore.Diagnostics {
 
  public class FunctionInfo : IFormattable{
        
        private static Regex regex = null;
        
        private string ns; 			// Namespace
        private string cl; 			// Class
        private string method; 		// Methode
        private string parameter; 	// Parameter der Methode
        
        public string Namespace{ get{ return ns; } }
        public string Class{ get{ return cl; } }
        public string Method{ get{ return method; } }
        public string Parameter{ get{ return parameter; } }

        public FunctionInfo(int offset) {
            FunctionInfo fi = getFunctionInfo(1 + offset);
            this.ns = fi.ns;
            this.cl = fi.cl;
            this.method = fi.method;
            this.parameter = fi.parameter;
        }
        
        public FunctionInfo(string stackTraceLine) {
            
            if(regex == null) {
                //                   bei/at [|           namespace              |  .] |    class | . |       [.]method                                       |  (|       parameter     |)
                regex = new Regex("^   \\w* ((?<namespace>[\\w_\\.][\\w_\\.1-9]*)\\.|)(?<class>.*)\\.(?<method>\\.?(<\\.?[\\w_][\\w_1-9`]+>)?[\\w_][\\w_1-9]*)\\((?<parameter>[^\\)]*)\\)( .*)?$");
            }
        
            Match match = regex.Match(stackTraceLine);
            
            this.ns = match.Groups["namespace"].Value;
            this.cl = match.Groups["class"].Value;
            this.method = match.Groups["method"].Value;
            this.parameter = match.Groups["parameter"].Value;

            if(this.method=="")
                Log.Error("Can't parse FunctionInfo: " + stackTraceLine);
 
        }
        
        public string getInformations() {
            return 
                String.Format("{0}\n\tNamespace\t'{1}'\n\tClass\t\t'{2}'\n\tMethod\t\t'{3}'\n\tParameter\t'{4}'",
                    this.ToString(),
                    this.ns,
                    this.cl,
                    this.method,
                    this.parameter
                );
        }
        
        public override string ToString() {
            return this.ToString(true, true);
        }
        
        public string ToString(bool printNamespace, bool printParametes) {
            return
                (printNamespace && this.ns.Length > 0 ? this.ns + "." : "") + 	// Namespace
                this.cl + "." + this.method + 									// Class + Methode
                (printParametes ? "(" + this.parameter + ")" : "(...)")			// Parameter
            ;
        }

        public string ToString(string format, IFormatProvider provider) {
            return this.ToString(!format.Contains("n"), !format.Contains("p"));
        }
        
        public static FunctionInfo getFunctionInfo() {
            return getFunctionInfo(1);
        }
        
        public static FunctionInfo getFunctionInfo(int offset) {
            //offset -= 1;
            
            string[] stackTrace = (new StackTrace()).ToString().Replace("\r", "").Split('\n'); //Entferne "carriage return" (\r) und zerlege in Zeilen
            
            string function = stackTrace[offset]; //Nur die gewollte Methode
            
            FunctionInfo fi = new FunctionInfo(function);
            
            return fi;
        }
        
        /// <summary>
        /// Returns function signature from offset
        ///  - offset = 1:
        ///     function which called the function which called FunctionInfo.Function(...)
        ///  - offset = 0:
        ///     function which called FunctionInfo.Function(...)
        ///  - offset = -1:
        ///     FunctionInfo.Function(...)
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="printNamespace"></param>
        /// <param name="printParametes"></param>
        /// <returns></returns>
        public static String Function(int offset = 0, bool printNamespace = true, bool printParametes = false) {
            try {
                return getFunctionInfo(offset + 2).ToString(printNamespace, printParametes);
            } catch (Exception ex) {
                return "Error resolving Function : " + ex.ToString();
            }
        }
    }
}
