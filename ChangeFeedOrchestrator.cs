using System;
using System.IO;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.Cosmos.Fluent;

namespace CosmosChangeFeedExample
{
    public class ChangeFeedOrchestrator
    {
            static string _databaseName;
            static string _sourceContainerName;
            static string _accountConnectionString;
            static string  _leaseContainerName;

        public async Task Start(string accountConnectionString, string databaseName, string sourceContainerName)
        {
            SetupDatabaseAndContainerVariables(accountConnectionString, databaseName, sourceContainerName)            ;
            var client = BuildCosmosClient();
            await EnsureLeaseContainerExists(client);
            await StartChangeFeedProcessorAsync(client);
        }

        static void SetupDatabaseAndContainerVariables(string accountConnectionString, string databaseName, string sourceContainerName)
        {
            if (string.IsNullOrWhiteSpace(accountConnectionString))
            {
                throw new ArgumentNullException("Database or SourceContainer cannot be NULL");
            }
            if (string.IsNullOrWhiteSpace(databaseName) || string.IsNullOrWhiteSpace(sourceContainerName))
            {
                throw new ArgumentNullException("Database or SourceContainer cannot be NULL");
            }
            _accountConnectionString = accountConnectionString;
            _databaseName = databaseName;
            _sourceContainerName = sourceContainerName;
            _leaseContainerName = _sourceContainerName+"-lease";

            Logger.Info($"ChangeFeed monitoring set to Database: [{_databaseName}], Container: [{_sourceContainerName}]");
        }

        private async Task EnsureLeaseContainerExists(CosmosClient client)
        {

            Database database = await client.CreateDatabaseIfNotExistsAsync(_databaseName);
            await database.CreateContainerIfNotExistsAsync(new ContainerProperties(_leaseContainerName, "/id"));        }

        private async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync(CosmosClient cosmosClient)
        {
            Container leaseContainer = cosmosClient.GetContainer(_databaseName, _leaseContainerName);
            ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(_databaseName, _sourceContainerName)
                .GetChangeFeedProcessorBuilder<DbItem>(processorName: "changeFeedSample", onChangesDelegate: HandleChangesAsync)
                    .WithInstanceName("consoleHost")
                    .WithLeaseContainer(leaseContainer)
                    .Build();

            try
            {
                Logger.Info("Starting Change Feed Processor...");
                await changeFeedProcessor.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("Error!: " + ex.Message);
                throw;
            }
            Logger.Info("Change Feed Processor started.");
            return changeFeedProcessor;
        }

        private async Task HandleChangesAsync(ChangeFeedProcessorContext context, IReadOnlyCollection<DbItem> changes, CancellationToken cancellationToken)
        {
            Logger.Verbose($"Started handling changes for lease {context.LeaseToken}...");
            Logger.Verbose($"Change Feed request consumed {context.Headers.RequestCharge} RU.");
            // SessionToken if needed to enforce Session consistency on another client instance
            Logger.Verbose($"SessionToken ${context.Headers.Session}");

            // We may want to track any operation's Diagnostics that took longer than some threshold
            if (context.Diagnostics.GetClientElapsedTime() > TimeSpan.FromSeconds(1))
            {
                Logger.Verbose($"Change Feed request took longer than expected. Diagnostics:" + context.Diagnostics.ToString());
            }

            foreach (DbItem item in changes)
            {
                Logger.Info($"Detected operation for item [ Id:{item.id}, appId:{item.appId}, Firstname:{item.firstName} created: {item.creationTime.ToString("o")} ]");
                // Simulate some asynchronous operation
                await Task.Delay(10);
            }

            Logger.Info("Finished handling changes.");
        }

        private CosmosClient BuildCosmosClient()
        {
            return new CosmosClientBuilder(_accountConnectionString)
                .Build();
        }
    }
}