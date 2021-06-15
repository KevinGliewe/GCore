using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//Author : KEG
//Datum  : 16.05.2014 15:47:28
//Datei  : ConsoleLogger.cs


namespace GCore.Logging.Logger {
    public class ConsoleLogger : ILoggingHandler {

        #region Members
        public bool UseColor = true;
        LogEntry.LogTypes _logTypes;
        #endregion

        #region Events
        #endregion

        #region Initialization
        public ConsoleLogger(LogEntry.LogTypes logtypes = LogEntry.LogTypes.NoDebug) {
            this._logTypes = logtypes;
        }
        #endregion

        #region Finalization
        ~ConsoleLogger() {

        }
        #endregion

        #region Interface
        #endregion

        #region Interface(ILoggingHandler)
        public void Success(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Info(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Warn(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Debug(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Exaption(DateTime timestamp, string Message, Exception exception, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Error(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void Fatal(DateTime timestamp, string Message, System.Diagnostics.StackTrace Stacktrace, params object[] list) {}

        public void General(LogEntry logEntry) {
            _log(logEntry);
        }
        #endregion

        #region Tools

        private void _log(LogEntry logEntry) {
            if ((logEntry.LogType & this._logTypes) == 0)
                return;

            string Text = logEntry.TimeStamp.ToString("yyyy-MM-dd H:mm:ss.fff") + "\t";
            Text += logEntry.LogType.ToString() + "\t";
            Text += logEntry.Message + "\t";
            Text += logEntry.StackTrace.ToString().Split('\n')[1].Replace("\r", "").Trim() + "\t";
            Text += logEntry.Thread.Name + "\t";
            if (logEntry.Exception != null) Text += logEntry.Exception.ToString() + "\t";
            foreach (Object o in logEntry.Params)
                Text += o.ToString() + ";";

            XConsole.XConsole.WriteLine(this.GetColorByEntry(logEntry), Text);
        }

        protected ConsoleColor GetColorByEntry(LogEntry logEntry) {
            if(!this.UseColor)
                return Console.ForegroundColor;
            switch(logEntry.LogType) {
                case LogEntry.LogTypes.Debug:
                    return ConsoleColor.DarkGray;
                    break;
                case LogEntry.LogTypes.Error:
                    return XConsole.XConsole.ErrorColor;
                    break;
                case LogEntry.LogTypes.Exception:
                                        return XConsole.XConsole.ErrorColor;
                    break;
                case LogEntry.LogTypes.Fatal:
                                        return XConsole.XConsole.ErrorColor;
                    break;
                case LogEntry.LogTypes.Success:
                    return XConsole.XConsole.SuccessColor;
                    break;
                case LogEntry.LogTypes.Warn:
                    return XConsole.XConsole.WarningColor;
                    break;
            }
            return Console.ForegroundColor;
        }
        #endregion

        #region Browsable Properties
        #endregion

        #region NonBrowsable Properties
        #endregion
    }
}