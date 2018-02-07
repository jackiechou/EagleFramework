using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Tracing;
using log4net;

namespace Eagle.Core.Logging
{
    /// <summary>
    /// Trace writer to direct tracing to Log4Net in the Web API way.
    /// See these articles for more information about tracing in Web API:
    /// http://www.asp.net/web-api/overview/testing-and-debugging/tracing-in-aspnet-web-api
    /// http://blogs.msdn.com/b/roncain/archive/2012/04/12/tracing-in-asp-net-web-api.aspx
    /// </summary>
    public static class Logger
    {
        private static readonly ILog Log;
        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> LoggingLevel;
        static Logger()
        {
            //var loggerName = string.Empty;
            //var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            //if (declaringType != null)
            //{
            //    loggerName = declaringType.Name;
            //}
            //Log = LogManager.GetLogger(loggerName);
            Log = LogManager.GetLogger(typeof(Logger));
            log4net.Config.XmlConfigurator.Configure();

            // Set up mappings from Log4Net trace levels to the Web API equivalents
            LoggingLevel = new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>> 
            { 
                {TraceLevel.Info, Log.Info},
                {TraceLevel.Debug, Log.Debug},
                {TraceLevel.Error, Log.Error},
                {TraceLevel.Fatal, Log.Fatal},
                {TraceLevel.Warn, Log.Warn}
            });
        }
        
        /// <summary>
        /// Traces the specified request to Log4Net
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="category">The category.</param>
        /// <param name="level">The level.</param>
        /// <param name="traceAction">The trace action.</param>
        public static void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off)
                return;
 
            var record = new TraceRecord(request, category, level);
            traceAction(record);

            if (LoggingLevel.Value.ContainsKey(level))
            {
                LoggingLevel.Value[level](string.Format("{0};{1};{2};{3};{4};{5}", record.Operator, record.Operation, record.Category, record.Message, record.Kind, record.Level));
            }
        }

        public static void LogException(Exception ex)
        {
                ThreadContext.Properties["application"] = "WebApi";
                ThreadContext.Properties["host"] = System.Net.Dns.GetHostName();
                ThreadContext.Properties["logType"] = ex.GetType().FullName;
                ThreadContext.Properties["source"] = ex.Source ?? "N/A";
                ThreadContext.Properties["timeUtc"] = DateTime.UtcNow;
                ThreadContext.Properties["exceptionDetails"] = ex.ToXml("WebApi");
                Log.Error(ex.Message, ex);

        }

        //#region Log4net

        //public void SetupLog4net()
        //{
        //    var hierarchy = (Hierarchy)LogManager.GetRepository();

        //    var rollingAppender = GetRollingAppender();
        //    var consoleAppender = GetConsoleAppender();

        //    hierarchy.Root.AddAppender(consoleAppender);
        //    hierarchy.Root.AddAppender(rollingAppender);

        //    hierarchy.Root.Level = Level.Debug;
        //    hierarchy.Configured = true;

        //    var logger = LogManager.GetLogger(typeof(LogConfiguration));
        //    logger.Info($"Init Log4net successfully.");
        //}

        //private ConsoleAppender GetConsoleAppender()
        //{
        //    var patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = "[%-40.60logger{3}] - %message%newline";
        //    patternLayout.ActivateOptions();

        //    var consoler = new ConsoleAppender();
        //    consoler.Layout = patternLayout;
        //    consoler.Threshold = Level.Info;
        //    return consoler;
        //}

        //private RollingFileAppender GetRollingAppender()
        //{
        //    var patternLayout = new PatternLayout();
        //    patternLayout.ConversionPattern = "[%date{yyyy-MM-dd HH:mm:ss.fff}] [%-5level] [%-60.60logger{3}] - %message%newline";
        //    patternLayout.ActivateOptions();

        //    var roller = new RollingFileAppender();
        //    roller.AppendToFile = false;
        //    roller.File = @"Logs\Log4net.log";
        //    roller.Layout = patternLayout;
        //    roller.MaxSizeRollBackups = 5;
        //    roller.MaximumFileSize = "2MB";
        //    roller.RollingStyle = RollingFileAppender.RollingMode.Size;
        //    roller.StaticLogFileName = true;
        //    roller.ActivateOptions();
        //    return roller;
        //}

        //#endregion

        #region  Public Methods

        /// <summary>
        ///    Log a message object with the log4net.Core.Level.Debug level.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(object message)
        {
            Log.Debug(message);
        }

        /// <summary>
        ///    Logger.a message object with the Logger.net.Core.Level.Debug level.
        /// </summary>
        /// <param name="format">The string message format.</param>
        /// <param name="args">The list of params.</param>
        public static void DebugFormat(string format, params object[] args)
        {
            Log.DebugFormat(format, args);
        }

        /// <summary>
        ///  Logs a message object with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(object message)
        {
            Log.Error(message);
        }

        /// <summary>
        ///    Log a exception with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        /// <summary>
        ///  Log a message object with the log4net.Core.Level.Error level including the
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex">The ex.</param>
        public static void Error(object message, Exception ex)
        {
            Log.Error(message, ex);
        }

        /// <summary>
        ///    Log a exception with the log4net.Core.Level.Error level.
        /// </summary>
        /// <param name="format">The string message format.</param>
        /// <param name="args">The list of params.</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        /// <summary>
        ///  Logs a message object with the log4net.Core.Level.Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(object message)
        {
            Log.Info(message);
        }

        /// <summary>
        ///  Logger. a message object with the log4net.Core.Level.Info level.
        /// </summary>
        /// <param name="format">The string message format.</param>
        /// <param name="args">The list of params.</param>
        public static void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(format, args);
        }

        /// <summary>
        ///  Logs a message object with the log4net.Core.Level.Info Warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warning(object message)
        {
            Log.Warn(message);
        }

        /// <summary>
        ///  Logs a message object with the log4net.Core.Level.Info Warning.
        /// </summary>
        /// <param name="format">The string message format.</param>
        /// <param name="args">The list of params.</param>
        public static void WarningFormat(string format, params object[] args)
        {
            Log.WarnFormat(format, args);
        }

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }

        /// <summary>
        /// Log a exception with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Fatal(Exception ex)
        {
            Log.Fatal(ex);
        }

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Fatal level including the
        ///  stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Fatal(object message, Exception exception)
        {
            Log.Fatal(message, exception);
        }

        /// <summary>
        /// Log a message object with the log4net.Core.Level.Fatal level.
        /// </summary>
        /// <param name="format">The string message format.</param>
        /// <param name="args">The list of params.</param>
        public static void FatalFormat(string format, params object[] args)
        {
            Log.FatalFormat(format, args);
        }

        #endregion
    }
}