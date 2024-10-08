using NLog;
using HavucDent.Common.Logging;

namespace HavucDent.Web.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }
    }
}