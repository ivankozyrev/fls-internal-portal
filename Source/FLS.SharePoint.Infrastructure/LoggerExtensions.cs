using System;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace FLS.SharePoint.Infrastructure
{
    public static class LoggerExtensions
    {
        private static readonly string categoryDebug = "FLS.SharePoint/Debug";
        private static readonly string categoryError = "FLS.SharePoint/Error";

        public static void Debug(this ILogger logger, string message)
        {
            logger.TraceToDeveloper(message, categoryDebug);
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            logger.TraceToDeveloper(ex, categoryError);
        }

        public static void DebugFormat(this ILogger logger, string format, params object[] args)
        {
            var message = string.Format(format, args);
            logger.TraceToDeveloper(message, categoryDebug);
        }
    }
}