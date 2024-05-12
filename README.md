# ApiTemplate
## Description
A template that can easily be cloned and modified to quickly stand-up an API.

## Components
This template consists of the following components:

* **[Api](./Api/README.md)** - The application API
* **[App](./App/README.md)** - The application business logic
* **[AppCore](./AppCore/README.md)**: The applicatinon core
* **[AppDi](./AppDi/README.md)** - The application's dependency injection
* **[AppTests](./AppTests/README.md)** - Unit tests for the application
* **[Db](./Db/README.md)** - Interfaces to abstract interaction with permanent storage like a database
* **[MongoDB](./MongoDb/README.md)** - Permanent storage implementation using a document data store of MongoDB

## Setup
### MongoDB
The setup requires a username and password be set. To achieve this set an environment variable called `MONGO_USERNAME` to be the username for the database, and `MONGO_PASSWORD` to be the password for the database.  If you have just set or changed these you will likely need to restart Visual Studio.