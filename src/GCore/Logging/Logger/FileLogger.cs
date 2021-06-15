using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GCore.Logging.Logger {
    public class FileLogger :ILoggingHandler {

        #region ILoggingHandler Member

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

        private void _log(LogEntry logEntry) {
            string Text = logEntry.TimeStamp.ToString("yyyy-MM-dd H:mm:ss.fff") + "\t";
            Text += logEntry.LogType.ToString() + "\t";
            Text += logEntry.Message + "\t";
            Text += logEntry.StackTrace.ToString().Split('\n')[1].Replace("\r", "").Trim() + "\t";
            Text += logEntry.Thread.Name + "\t";
            if (logEntry.Exception != null) Text += logEntry.Exception.ToString() + "\t";
            foreach (Object o in logEntry.Params)
                Text += o.ToString() + ";";

            lock (_outFile) {
                using (StreamWriter w = File.AppendText(_outFile)) {
                    w.WriteLine(Text);
                }
            }
        }

        private string _outFile;

        public LogEntry.LogTypes LogFilter;

        public FileLogger(string file, LogEntry.LogTypes logFilter = LogEntry.LogTypes.All) {

            FileInfo f = new FileInfo(file);
            if (f.Exists)
                if (f.IsReadOnly) throw new FieldAccessException(file);
            _outFile = new FileInfo(file).FullName;
            LogFilter = logFilter;
        }
    }
}
