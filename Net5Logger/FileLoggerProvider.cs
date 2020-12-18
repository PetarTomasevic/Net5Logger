using Microsoft.Extensions.Logging;
using System;

namespace Net5Logger
{
    public class FileLoggerProvider : ILoggerProvider, IDisposable
    {
        private FileLogger _FileLogger;
        private bool _disposed;
        public string FilePath { get; set; }

        public FileLoggerProvider(string filePath)
        {
            FilePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            _FileLogger = new FileLogger(FilePath);
            return _FileLogger;
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

                _FileLogger.Dispose();
                _disposed = true;
            }
        }
    }
}