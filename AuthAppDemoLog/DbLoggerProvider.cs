using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace AuthAppDemoLog
{
    /*
    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;

        public DbLoggerProvider(IOptions<DbLoggerOptions> _options)
        {
            Options = _options.Value; // Stores all the options.
        }

        /// <summary>
        /// Creates a new instance of the db logger.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }
    */

    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;
        private readonly string _logTable;
        private readonly ConcurrentDictionary<string, DbLogger> _loggers = new ConcurrentDictionary<string, DbLogger>();

        public DbLoggerProvider(string connectionString, string logTable)
        {
            _connectionString = connectionString;
            _logTable = logTable;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DbLogger(_connectionString, _logTable));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }

}
