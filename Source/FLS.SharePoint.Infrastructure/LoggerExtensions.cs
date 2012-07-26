using Microsoft.Practices.SharePoint.Common.Logging;

namespace FLS.SharePoint.Infrastructure
{
    public static class LoggerExtensions
    {
        private static readonly string category = "FLS.SharePoint/Debug";

        public static void Debug(this ILogger logger, string message)
        {
            logger.TraceToDeveloper(message, category);
        }

        public static void DebugFormat(this ILogger logger, string format, params object[] args)
        {
            var message = string.Format(format, args);
            logger.TraceToDeveloper(message, category);
        }
    }
}