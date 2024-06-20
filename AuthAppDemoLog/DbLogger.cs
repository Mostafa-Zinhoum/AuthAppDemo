    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
using System.Collections.Generic;
using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

namespace AuthAppDemoLog
{
    /*
    /// <summary>
    /// Writes a log entry to the database.
    /// </summary>
    public class DbLogger : ILogger
    {
        /// <summary>
        /// Instance of <see cref="DbLoggerProvider" />.
        /// </summary>
        private readonly DbLoggerProvider _dbLoggerProvider;

        /// <summary>
        /// Creates a new instance of <see cref="FileLogger" />.
        /// </summary>
        /// <param name="fileLoggerProvider">Instance of <see cref="FileLoggerProvider" />.</param>
        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Whether to log the entry.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }


        /// <summary>
        /// Used to log the entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">An instance of <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event's ID. An instance of <see cref="EventId"/>.</param>
        /// <param name="state">The event's state.</param>
        /// <param name="exception">The event's exception. An instance of <see cref="Exception" /></param>
        /// <param name="formatter">A delegate that formats </param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                // Don't log the entry if it's not enabled.
                return;
            }

            var threadId = Thread.CurrentThread.ManagedThreadId; // Get the current thread ID to use in the log file. 

            // Store record.
            using (var connection = new SqlConnection(_dbLoggerProvider.Options.ConnectionString))
            {
                connection.Open();

                // Add to database.

                // LogLevel
                // ThreadId
                // EventId
                // Exception Message (use formatter)
                // Exception Stack Trace
                // Exception Source

                var values = new JObject();

                if (_dbLoggerProvider?.Options?.LogFields?.Any() ?? false)
                {
                    foreach (var logField in _dbLoggerProvider.Options.LogFields)
                    {
                        switch (logField)
                        {
                            case "LogLevel":
                                if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                                {
                                    values["LogLevel"] = logLevel.ToString();
                                }
                                break;
                            case "ThreadId":
                                values["ThreadId"] = threadId;
                                break;
                            case "EventId":
                                values["EventId"] = eventId.Id;
                                break;
                            case "EventName":
                                if (!string.IsNullOrWhiteSpace(eventId.Name))
                                {
                                    values["EventName"] = eventId.Name;
                                }
                                break;
                            case "Message":
                                if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                                {
                                    values["Message"] = formatter(state, exception);
                                }
                                break;
                            case "ExceptionMessage":
                                if (exception != null &&
                                    !string.IsNullOrWhiteSpace(exception.Message))
                                {
                                    values["ExceptionMessage"] = exception?.Message;
                                }
                                break;
                            case "ExceptionStackTrace":
                                if (exception != null
                                    && !string.IsNullOrWhiteSpace(exception.StackTrace))
                                {
                                    values["ExceptionStackTrace"] = exception?.StackTrace;
                                }
                                break;
                            case "ExceptionSource":
                                if (exception != null
                                    && !string.IsNullOrWhiteSpace(exception.Source))
                                {
                                    values["ExceptionSource"] = exception?.Source;
                                }
                                break;
                        }
                    }
                }
                if (state is CustomLogState customState)
                {
                    var headers = new JObject();
                    foreach (var header in customState.RequestHeaders)
                    {
                        headers[header.Key] = header.Value;
                    }
                    values["RequestHeaders"] = headers;
                }

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = string.Format("INSERT INTO {0} ([Values], [Created]) " +
                        "VALUES (@Values, @Created)",
                        _dbLoggerProvider.Options.LogTable);

                    command.Parameters.Add(new SqlParameter("@Values",
                        JsonConvert.SerializeObject(values, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            DefaultValueHandling = DefaultValueHandling.Ignore,
                            Formatting = Formatting.None
                        }).ToString()));
                    command.Parameters.Add(new SqlParameter("@Created", DateTimeOffset.Now));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
    */

    public class DbLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _logTable;

        public DbLogger(string connectionString, string logTable)
        {
            _connectionString = connectionString;
            _logTable = logTable;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var threadId = Thread.CurrentThread.ManagedThreadId;
            var message = formatter(state, exception);

            var logEntry = new LogEntry
            {
                LogLevel = logLevel.ToString(),
                EventId = eventId.Id.ToString(),
                EventName = eventId.Name,
                Message = message,
                ExceptionMessage = exception?.Message,
                ExceptionStackTrace = exception?.StackTrace,
                ExceptionSource = exception?.Source,
                Created = DateTime.Now
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(
                    $"INSERT INTO {_logTable} (LogLevel, EventId, EventName, Message, ExceptionMessage, ExceptionStackTrace, ExceptionSource, Created) " +
                    "VALUES (@LogLevel, @EventId, @EventName, @Message, @ExceptionMessage, @ExceptionStackTrace, @ExceptionSource, @Created)", connection);

                command.Parameters.AddWithValue("@LogLevel", logEntry.LogLevel);
                command.Parameters.AddWithValue("@EventId", logEntry.EventId??string.Empty);
                command.Parameters.AddWithValue("@EventName", logEntry.EventName ?? string.Empty);
                command.Parameters.AddWithValue("@Message", logEntry.Message ?? string.Empty);
                command.Parameters.AddWithValue("@ExceptionMessage", logEntry.ExceptionMessage ?? string.Empty);
                command.Parameters.AddWithValue("@ExceptionStackTrace", logEntry.ExceptionStackTrace ?? string.Empty);
                command.Parameters.AddWithValue("@ExceptionSource", logEntry.ExceptionSource ?? string.Empty);
                command.Parameters.AddWithValue("@Created", logEntry.Created);

                command.ExecuteNonQuery();
            }
        }
    }
    
}

