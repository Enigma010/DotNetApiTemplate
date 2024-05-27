# ApiTemplate
## Description
A template that can easily be cloned and modified to quickly stand-up an API.

## Components
This template consists of the following components:

* **[Api](./Api/README.md)** - The application API
* **[App](./App/README.md)** - The application business logic
* **[AppCore](./AppCore/README.md)**: The application core
* **[AppTests](./AppTests/README.md)** - Unit tests for the application
* **[Db](./Db/README.md)** - Interfaces to abstract interaction with permanent storage like a database
* **[EventBus](./EventBus/README.md)** - Infrastructure used to work with the event bus
* **[MongoDB](./MongoDb/README.md)** - Permanent storage implementation using a document data store of MongoDB

## Setup
### EventBus
The settings for the EventBus can be found in `Api/appsettings.Development.json` under the `EventBus` node. No additional configuration is necessary to run the EventBus with the default settings.

### MongoDB
The settings for MongoDB can be found in the `Api\appsettings.Development.json` under the `Db` node. No additional configuraiton is necessary to run the MongoDB with the default settings.

Next you'll need to create/modify the `Api\.env` file. Create the file with the following content to start:

```
MONGO_USERNAME: username
MONGO_PASSWORD: password
MONGO_PORT: 27017
```

Change the `username` value to be the same value in the `Api\appsettings.Development.json` `Db` node, sub-node `Username` value.  

Change the `password` value to be the same value in the `Api\appsettings.Development.json` `Db` node, sub-node `Password` value.

Next you will also need to generate a replication key file so that MongoDB can handle rolling back transactions.  To do this open up a **wsl** command window and change directories to `MongoDb` from there run the command:

```
./lrun.sh
```

The output of this command is a file `MongoDb\Iac\Output\replica.key` this will be used when you start the docker containers for the API.

## Running the API
To run the API you need to do the following:

1. Open up a **wsl** command prompt and change directories to `EventBus`.  Then issue the command:

```
./lrun.sh
```

This will start a local version of the event bus on your local machine.

2. Open up Visual Studio and open the solution `Api\Api.sln`.  Under the start-up projects select `docker-compose` and press the play button. It should open up a swagger API page that you can use to run HTTP REST commands.
