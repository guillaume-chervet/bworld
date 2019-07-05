using System;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Demo.Log
{
    public class MongoLogger : ILogger
    {
        private readonly string applicationName;
        private readonly string environmentName;
        private readonly IMongoCollection<Demo.Log.Log> logCollection;
        private readonly string name;
        private Func<string, LogLevel, bool> filter;

        public MongoLogger(string name, Func<string, LogLevel, bool> filter,
            string connectionString,
            string environmentName,
            string applicationName, string databaseName)
        {
            Filter = filter ?? ((category, logLevel) => true);
            this.environmentName = environmentName;
            this.applicationName = applicationName;
            this.name = name;
            var db = new MongoClient(connectionString).GetDatabase(databaseName ?? "MongoDbLog");
            logCollection = db.GetCollection<Demo.Log.Log>("site.logs");
        }

        private Func<string, LogLevel, bool> Filter
        {
            get => filter;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                filter = value;
            }
        }

	    /// <summary>
	    ///     Writes a log entry.
	    /// </summary>
	    /// <param name="logLevel">Entry will be written on this level.</param>
	    /// <param name="eventId">Id of the event.</param>
	    /// <param name="state">The entry to be written. Can be also an object.</param>
	    /// <param name="exception">The exception related to this entry.</param>
	    /// <param name="formatter">
	    ///     Function to create a <c>string</c> message of the <paramref name="state" /> and
	    ///     <paramref name="exception" />.
	    /// </param>
	    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var title = formatter(state, exception);

            var exceptinon = exception?.ToString();

            var log = new Demo.Log.Log
            {
                Message = title,
                Exception = exceptinon,
                Level = logLevel,
                ApplicationName = applicationName,
                Environment = environmentName
            };

            logCollection.InsertOne(log);
        }

	    /// <summary>
	    ///     Checks if the given <paramref name="logLevel" /> is enabled.
	    /// </summary>
	    /// <param name="logLevel">level to be checked.</param>
	    /// <returns><c>true</c> if enabled.</returns>
	    public bool IsEnabled(LogLevel logLevel)
        {
            return Filter(name, logLevel);
        }

	    /// <summary>
	    ///     Begins a logical operation scope.
	    /// </summary>
	    /// <param name="state">The identifier for the scope.</param>
	    /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
	    public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}