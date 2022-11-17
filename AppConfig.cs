using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CosmosChangeFeedExample
{
    public static class AppConfig
    {
        public static IConfigurationRoot Values { get; private set; }
        static AppConfig()
        {
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appConfigBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Values = appConfigBuilder.Build();
        }

        public static string DatabaseId { get { return Values["DatabaseId"]; } }
        public static string SourceContainerId { get { return Values["SourceContainerId"]; } }
        public static string CosmosDbAccountConnectionString { get { return Values["CosmosDbAccountConnectionString"]; } }
        public static int LeaseContainerThroughput { get { return GetSafeInt( Values["LeaseContainerThroughput"],400); } }
        private static int GetSafeInt(string key, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(Values[key]))
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt32(Values[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

    }
    
}