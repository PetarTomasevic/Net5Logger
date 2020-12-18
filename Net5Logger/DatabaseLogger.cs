using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Net5Logger
{
    public class DatabaseLogger : ILogger, IDisposable
    {
        private readonly SqlConnection _dbCon;
        private bool _disposed;

        public DatabaseLogger(string connStr)
        {
            _dbCon = new SqlConnection();
            _dbCon.ConnectionString = connStr;
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
                var queryString = $"INSERT INTO Logs ([LogLevel],[EventId],[State],[Exception],[Message],[ProccesTimeMiliSeconds],[Host]) VALUES('{logLevel}','{eventName}','{stateName}','{exceptionInfo}','{formatInfo}','{proccessTime}','{host}'); ";

                using (SqlConnection connection = new SqlConnection(_dbCon.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
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
                _dbCon.Dispose();
                _disposed = true;
            }
        }
    }
}