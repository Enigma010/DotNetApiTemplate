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
The setup requires a few environment variables to be able to function. 

| Environment Variable | Description | Default Value |
| - | - | - |
| EVENT_BUS_HOST | The host running RabbitMQ | localhost |
| EVENT_BUS_USERNAME | The username used to connect to RabbitMQ | guest |
| EVENT_BUS_PASSWORD | The password used to connect to RabbitMq | guest |

### MongoDB
MongoDB requires a few things to be setup to function. The first is you need to setup the following environment variables:

| Environment Variable | Description |
| - | - |
| MONGO_USERNAME | The name of the user for MongoDb |
| MONGO_PASSWORD | The password of the user for MongoDb |

If you have just set or changed these you will likely need to restart Visual Studio.

After you have setup the environment variables you will also need to generate a replication key file so that MongoDB can handle rolling back transactions.  To do this open up a **wsl** command window and change directories to `MongoDb` from there run the command:

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
