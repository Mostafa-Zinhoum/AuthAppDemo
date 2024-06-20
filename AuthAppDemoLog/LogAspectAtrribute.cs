using AspectInjector.Broker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime;

namespace AuthAppDemoLog
{
    [Aspect(Scope.Global)]
    [Injection(typeof(LogAspect))]
    public class LogAspect : Attribute
    {
        private readonly ILogger<LogAspect> _logger;
        /*
        public LogAspect()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            _logger = loggerFactory.CreateLogger<LogAspect>();
        }
        */

        public LogAspect()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureLogging(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _logger = serviceProvider.GetRequiredService<ILogger<LogAspect>>();
        }

        private void ConfigureLogging(IServiceCollection services)
        {
            // Create a configuration builder to load appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configurationBuilder.Build();

            // Bind DbLoggerOptions from configuration
            var dbLoggerOptions = new DbLoggerOptions();
            configuration.GetSection("Logging:Database:Options").Bind(dbLoggerOptions);

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.AddProvider(new DbLoggerProvider(dbLoggerOptions?.ConnectionString , dbLoggerOptions?.LogTable ));
            });
        }

        [Advice(Kind.Before, Targets = Target.Method)]
        public void LogMethodEntry(
            [Argument(Source.Type)] Type type,
            [Argument(Source.Method)] MethodBase method,
            [Argument(Source.Arguments)] object[] args)
        {
            _logger.LogInformation($"Entering {type.FullName}.{method.Name} with arguments: {string.Join(", ", args.Select(a => a != null ? a.ToString() : "null"))}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void LogMethodExit(
            [Argument(Source.Type)] Type type,
            [Argument(Source.Method)] MethodBase method,
            [Argument(Source.ReturnValue)] object returnValue)
        {
            _logger.LogInformation($"Exiting {type.FullName}.{method.Name} with return value: {(returnValue != null ? returnValue.ToString() : "void")}");
        }
    }
}
