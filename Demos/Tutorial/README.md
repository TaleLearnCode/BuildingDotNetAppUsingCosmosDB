You will need to add the following section to your appsettings.json file:

```
  "CosmosDb": {
    "Account": "<Endpoint URI of your Azure Cosmos account>",
    "Key": "<PRIMARY KEY of your Azure Cosmos account",
    "DatabaseName": "Tasks",
    "ContainerName": "Item"
  }