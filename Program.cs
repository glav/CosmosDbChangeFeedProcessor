using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CosmosChangeFeedExample
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Logger.Info("Starting ChageFeedOrchestrator...");
            var feedOrchestrator = new ChangeFeedOrchestrator();
            await feedOrchestrator.Start(AppConfig.CosmosDbAccountConnectionString, AppConfig.DatabaseId,AppConfig.SourceContainerId);

            Logger.Info("Started. Waiting on change feed....press <ENTER> to Quit"); 
            Console.Read();
        }
    }

}