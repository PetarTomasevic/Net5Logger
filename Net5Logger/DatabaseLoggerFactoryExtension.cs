using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Net5Logger
{
    public static class DatabaseLoggerFactoryExtension
    {
        public static ILoggingBuilder AddDatabaseLogging(this ILoggingBuilder builder, string connStr)
        {
            builder.Services.AddSingleton<ILoggerProvider>(_ => new DatabaseLoggerProvider(connStr));
            return builder;
        }
    }
}