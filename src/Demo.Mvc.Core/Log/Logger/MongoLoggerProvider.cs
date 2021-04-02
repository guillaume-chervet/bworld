using System;
using Microsoft.Extensions.Logging;

namespace Demo.Log
{
    public class MongoLoggerProvider : ILoggerProvider
    {
        private readonly string applicationName;
        private readonly MongoConfiguration configuration;
        private readonly string environmentName;
        private readonly Func<string, LogLevel, bool> filter;

        public MongoLoggerProvider(Func<string, LogLevel, bool> filter,
            MongoConfiguration configuration,
            string applicationName, string environmentName)
        {
            this.filter = filter;
            this.configuration = configuration;
            this.applicationName = applicationName;
            this.environmentName = environmentName;
        }

        public void Dispose()
        {
        }

	    /// <summary>
	    ///     Creates a new <see cref="ILogger" /> instance.
	    /// </summary>
	    /// <param name="categoryName">The category name for messages produced by the logger.</param>
	    /// <returns></returns>
	    public ILogger CreateLogger(string categoryName)
        {
            return new MongoLogger(categoryName, filter, configuration.ConnectionString, environmentName,
                applicationName, configuration.DatabaseName);
        }
    }
}