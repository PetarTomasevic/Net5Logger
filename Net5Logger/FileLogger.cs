using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net5Logger
{
    public class FileLogger : ILogger, IDisposable
    {
        private readonly string _folderPath;
        private bool _disposed;

        public FileLogger(string folderPath)
        {
            _folderPath = folderPath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                var loggerLevel = logLevel;
                var eventName = eventId.Id + "-" + eventId.Name;

                var stateName = state.GetType().FullName;
                var exceptionInfo = exception?.Message.Replace("'", "").ToString() + exception?.StackTrace.ToString();
                var formatInfo = formatter(state, exception);
                var proccessTime = "";
                var host = "";
                if (state is IReadOnlyList<KeyValuePair<string, object>> values && values.FirstOrDefault(kv => kv.Key == "ElapsedMilliseconds").Value is double miliseconds)
                {
                    proccessTime = miliseconds.ToString();
                }
                if (state is IReadOnlyList<KeyValuePair<string, object>> hostValues && hostValues.FirstOrDefault(kv => kv.Key == "Host").Value is string hostValue)
                {
                    host = hostValue.ToString();
                }
                string line = DateTime.Now.Date.ToShortDateString() + " : " + loggerLevel.ToString() + " EventName: " + eventName + " State Name: " + stateName + " Exception Info: " + exceptionInfo + "Format Info : " + formatInfo + " Execution Time: " + proccessTime + " Host: " + host;
                var filePath = _folderPath + "/" + DateTime.UtcNow.ToShortDateString() + ".txt";
                if (!System.IO.Directory.Exists(_folderPath))
                {
                    System.IO.Directory.CreateDirectory(_folderPath);
                }

                using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                {
                    streamWriter.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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

                _disposed = true;
            }
        }
    }
}