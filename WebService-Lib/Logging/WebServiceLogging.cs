using Microsoft.Extensions.Logging;

namespace WebService_Lib.Logging
{
    public class WebServiceLogging
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static ILoggerFactory LoggerFactory {get;} = new LoggerFactory();
        public static ILogger CreateLogger<T>() =>
            LoggerFactory.CreateLogger<T>();
    }
}