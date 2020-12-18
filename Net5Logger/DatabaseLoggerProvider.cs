using Microsoft.Extensions.Logging;
using System;

namespace Net5Logger
{
    public class DatabaseLoggerProvider : ILoggerProvider, IDisposable
    {
        private DatabaseLogger _DatabaseLogger;
        public string ConnectionString { get; set; }
        private bool _disposed;

        public DatabaseLoggerProvider(string connStr)
        {
            ConnectionString = connStr;
        }

        public ILogger CreateLogger(string categoryName)
        {
            _DatabaseLogger = new DatabaseLogger(ConnectionString);
            return _DatabaseLogger;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }

                _DatabaseLogger.Dispose();
                _disposed = true;
            }
        }
    }
}