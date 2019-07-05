using System;
using Demo.Log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Mvc.Core.Logging
{
    public static class MongoLoggerExtension
    {
        public static ILoggerFactory AddMongoDb(this ILoggerFactory factory, MongoConfiguration configuration,
            string applicationName, string environmentName)
        {
            if (string.IsNullOrEmpty(applicationName)) throw new ArgumentNullException(nameof(applicationName));

            if (string.IsNullOrEmpty(environmentName)) throw new ArgumentNullException(nameof(environmentName));

            ILoggerProvider provider = new MongoLoggerProvider((n, l) => l >= configuration.MinLevel, configuration,
                applicationName, environmentName);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddMongoDb(this ILoggerFactory factory, Func<string, LogLevel, bool> filter,
            MongoConfiguration configuration, string applicationName, string environmentName)
        {
            if (string.IsNullOrEmpty(applicationName)) throw new ArgumentNullException(nameof(applicationName));

            if (string.IsNullOrEmpty(environmentName)) throw new ArgumentNullException(nameof(environmentName));

            ILoggerProvider provider = new MongoLoggerProvider(filter, configuration, applicationName, environmentName);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddMongoDb(this ILoggerFactory factory, MongoConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            ILoggerProvider provider = new MongoLoggerProvider((n, l) => l >= configuration.MinLevel, configuration,
                hostingEnvironment.ApplicationName, hostingEnvironment.EnvironmentName);

            factory.AddProvider(provider);

            return factory;
        }

        public static ILoggerFactory AddMongoDb(this ILoggerFactory factory, Func<string, LogLevel, bool> filter,
            MongoConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            ILoggerProvider provider = new MongoLoggerProvider(filter, configuration,
                hostingEnvironment.ApplicationName, hostingEnvironment.EnvironmentName);

            factory.AddProvider(provider);

            return factory;
        }
    }
}