using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeToString;


namespace GCore.Logging.Expressions
{
    public static class LogExpr
    {
        public static (string ExprStr, object ExprRes) EvalExpression(this Expression<Func<Object>> exprFunc)
        {
            var exprStr = new String(exprFunc.ToString("C#").Skip(5).ToArray());
            var exprRes = exprFunc.Compile().Invoke();

            return (exprStr, exprRes);
        }

        private static void DoLog(LogEntry.LogTypes logType, string message, StackTrace stacktrace,
            Expression<Func<Object>> exprFunc, Exception exception)
        {
            var res = exprFunc.EvalExpression();
            message += ": " + res.ExprStr + " == " + res.ExprRes;
            GCore.Logging.Log.DoLog(logType, DateTime.Now, message, new StackTrace(), new[] { res.ExprRes }, exception);
        }

        public static void Success(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Success, message, new StackTrace(), exprFunc, null);
        }

        public static void Info(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Info, message, new StackTrace(), exprFunc, null);
        }

        public static void Warn(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Warn, message, new StackTrace(), exprFunc, null);
        }

        public static void Debug(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Debug, message, new StackTrace(), exprFunc, null);
        }

        public static void Exception(string message, Exception excaption, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Exception, message, new StackTrace(), exprFunc, excaption);
        }

        public static void Error(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Error, message, new StackTrace(), exprFunc, null);
        }

        public static void Fatal(string message, Expression<Func<Object>> exprFunc)
        {
            DoLog(LogEntry.LogTypes.Fatal, message, new StackTrace(), exprFunc, null);
        }
    }
}