# ApiTemplate
## Description
A template that can easily be cloned and modified to quickly stand-up an API.

## Components
This template consists of the following components:

* **[Api](./Api/README.md)** - The application API
* **[App](./App/README.md)** - The application business logic
* **[AppCore](./AppCore/README.md)**: The applicatinon core
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
The setup requires a username and password be set. To achieve this set an environment variable called `MONGO_USERNAME` to be the username for the database, and `MONGO_PASSWORD` to be the password for the database.  If you have just set or changed these you will likely need to restart Visual Studio.