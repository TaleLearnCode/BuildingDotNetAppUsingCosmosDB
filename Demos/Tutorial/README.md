# Building a .NET Application Using Azure Cosmos DB

This demo code is used to show how to develop an application that stores its data in Azure Cosmos DB.  Here is a description of the demo components along with instructions in order to run the demos on your machine.

### Domain
Defines the domain objects for the to do project.

### Services
Provides a set of services for working with the to do data.  Be sure to look at the Common class as that is where most of the access to the Azure Cosmos DB database is performed.

### Tutorial
This is a simple ASP.NET Core MVC website that allows a user to view their to do items.

You will need to add the following section to your appsettings.json file:

```
  "CosmosDb": {
    "Account": "<Endpoint URI of your Azure Cosmos account>",
    "Key": "<PRIMARY KEY of your Azure Cosmos account>",
    "DatabaseName": "Tasks",
    "ContainerName": "Item"
  }
```

### ChangeFeedMonitor
This is a set of Azure Functions that perform actions based upon changes within the Azure Cosmos DB database.  Something to note on this project is that we are using dependency injection - this is not done by default.

The *ChangeFeedMonitor* function copies to do items into an Azure Storage account which is used for archival purposes.

The *MetadataChangeFeed* function updates to do items associated to an item status.

In the appsettings.json file, you will need a CosmosConnectionString value with Azure Cosmos DB connection string.

### DemoBuilder
This is a simple console application used to populate the Azure Cosmos DB database with some initial data.

You will need to add a Settings class with the following structure:

```
public static class Settings
{
  public const string CosmosEndpoint = "<Endpoint URI of your Azure Cosmos account>";
  public const string CosmosKey = "<PRIMARY KEY of your Azure Cosmos account>";
  public const string CosmosDatabaseName = "Todo";
  public const string MetadataContainerName = "metadata";

  public const string ItemContainerName = "items";

  public const string StorageAccountName = "<Name of Azure Storage account>";
  public const string StorageAccountKey = "<Key of your Azure Storage account>";
  public const string ArchiveContainerName = "itemarchive";
}
```