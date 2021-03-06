﻿using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaleLearnCode.Todo.Services;

[assembly: FunctionsStartup(typeof(TaleLearnCode.Todo.Functions.Startup))]
namespace TaleLearnCode.Todo.Functions
{

	public class Startup : FunctionsStartup
	{

		public override void Configure(IFunctionsHostBuilder builder)
		{
			CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosConnectionString"));
			builder.Services.AddSingleton<IMetadataService>((s) => { return new MetadataService(cosmosClient, Settings.DatabaseName, Settings.MetadataContainerName, Environment.GetEnvironmentVariable("AzureCacheConnectionString")); });
			builder.Services.AddSingleton<ITodoService>(
				(s) =>
				{
					return new TodoService(
						cosmosClient,
						Settings.DatabaseName,
						Settings.ItemContainerName,
						Environment.GetEnvironmentVariable("ArchiveStorageAccount"),
						Environment.GetEnvironmentVariable("ArchiveStorageKey"),
						Environment.GetEnvironmentVariable("ArchiveContainer"));
				});
		}

	}

}