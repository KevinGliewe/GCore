using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GCore.Logging {
    public static class Log {

        public delegate void LogHandler(LogEntry logEntry);

        public static List<ILoggingHandler> LoggingHandler = new List<ILoggingHandler>();

        public static event LogHandler OnLog;



        private static void DoLog(LogEntry.LogTypes logType, DateTime timeStamp, string message, StackTrace stacktrace, object[] oparams, Exception exception) {
            DoLog(new LogEntry(
                    logType,
                    timeStamp,
                    message,
                    stacktrace,
                    oparams,
                    exception,
                    System.Threading.Thread.CurrentThread
                ));
        }

        private static void DoLog(LogEntry logEntry) {
            Task.Run(() =>
            {
                if (OnLog != null)
                    try
                    {
                        OnLog(logEntry);
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.Write(ex.ToString()); }
                foreach (ILoggingHandler handler in LoggingHandler)
                {
                    try
                    {
                        handler.General(logEntry);

                        switch (logEntry.LogType)
                        {
                            case LogEntry.LogTypes.Success:
                                handler.Success(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Info:
                                handler.Info(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Warn:
                                handler.Warn(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Debug:
                                handler.Debug(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Exception:
                                handler.Exaption(logEntry.TimeStamp, logEntry.Message, logEntry.Exception, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Error:
                                handler.Error(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                            case LogEntry.LogTypes.Fatal:
                                handler.Fatal(logEntry.TimeStamp, logEntry.Message, logEntry.StackTrace, logEntry.Params);
                                break;

                        }
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.Write(ex.ToString()); }
                }
            });
        }

        public static void Success(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Success,DateTime.Now, message,new StackTrace(), oparams, null);
        }

        public static void Info(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Info, DateTime.Now, message, new StackTrace(), oparams, null);
        }

        public static void Warn(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Warn, DateTime.Now, message, new StackTrace(), oparams, null);
        }

        public static void Debug(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Debug, DateTime.Now, message, new StackTrace(), oparams, null);
        }

        public static void Exception(string message, Exception excaption, params Object[] oparams){
            DoLog(LogEntry.LogTypes.Exception, DateTime.Now, message, new StackTrace(), oparams, excaption);
        }

        public static void Error(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Error, DateTime.Now, message, new StackTrace(), oparams, null);
        }

        public static void Fatal(string message,params Object[] oparams){
            DoLog(LogEntry.LogTypes.Fatal, DateTime.Now, message, new StackTrace(), oparams, null);
        }

        public static bool TryLog(Action Action, string Message = "", bool logOnSuccess = false) {
            try {
                Action();
                if (logOnSuccess) Log.Success(Message);
                return true;
            } catch (Exception ex) {
                Log.Exception(Message, ex);
                return false;
            }
        }

    
    }
}
