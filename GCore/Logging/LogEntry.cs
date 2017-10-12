using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace GCore.Logging {
    public class LogEntry {

        [Flags]
        public enum LogTypes {
            Success = 1,
            Info = 2,
            Warn = 4,
            Debug = 8,
            Exception = 16,
            Error = 32,
            Fatal = 64,
            NoDebug = Success | Info | Warn | Exception | Error | Fatal,
            All = Success | Info | Warn | Debug | Exception | Error | Fatal,
        }

        private LogTypes _logType;
        private DateTime _timeStamp;
        private string _message;
        private StackTrace _stacktrace;
        private object[] _params;
        private Exception _exception;
        private Thread _thread;

        public LogTypes LogType { get { return this._logType; } }
        public DateTime TimeStamp { get { return this._timeStamp; } }
        public string Message { get { return this._message; } }
        public StackTrace StackTrace { get { return this._stacktrace; } }
        public Object[] Params { get { return this._params; } }
        public Exception Exception { get { return this._exception; } }
        public Thread Thread { get { return this._thread; } }

        public LogEntry(LogEntry.LogTypes logType, DateTime timeStamp, string message, StackTrace stacktrace, object[] oparams, Exception exception, Thread thread) {
            _logType = logType;
            _timeStamp = timeStamp;
            _message = message;
            _stacktrace = stacktrace;
            _params = oparams;
            _exception = exception;
            _thread = thread;
        }
    }
}
