﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging;

namespace CloudFoundryOwin
{
    public static class LoggingConfig
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILoggerProvider LoggerProvider { get; set; }

        public static void Configure(IConfiguration configuration)
        {
            IServiceCollection serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            serviceCollection.AddLogging(builder => builder.AddDynamicConsole());
            LoggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}