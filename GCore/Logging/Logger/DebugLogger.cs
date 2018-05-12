using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GCore.Logging.Logger
{
    public class DebugLogger : ILoggingHandler {
        public void Debug(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void Error(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void Exaption(DateTime timestamp, string Message, Exception exception, StackTrace Stacktrace, params object[] list) {
        }

        public void Fatal(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void Info(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void Success(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void Warn(DateTime timestamp, string Message, StackTrace Stacktrace, params object[] list) {
        }

        public void General(LogEntry logEntry) {
            string Text = logEntry.TimeStamp.ToString("yyyy-MM-dd H:mm:ss.fff") + "\t";
            Text += logEntry.LogType.ToString() + "\t";
            Text += logEntry.Message + "\t";
            Text += logEntry.StackTrace.ToString().Split('\n')[1].Replace("\r", "").Trim() + "\t";
            Text += logEntry.Thread.Name + "\t";
            if(logEntry.Exception != null) Text += logEntry.Exception.ToString() + "\t";
            foreach(Object o in logEntry.Params)
                Text += o.ToString() + ";";

            System.Diagnostics.Debug.WriteLine(Text);
        }
    }
}
