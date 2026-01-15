# DotNetApiTemplate

## Description
A template that can easily be cloned and modified to quickly stand-up an API.

## Components
This template consists of the following components:

* **[Api](./Api/README.md)** - The application API
* **[App](./App/README.md)** - The application business logic
* **[App.Events](./App.Events/README.md)** - The events emitted from the application
* **[App.UnitTests](./App.UnitTests/README.md)** - Unit tests for the application
* **[EventBus](./EventBus/README.md)** - Infrastructure used to work with the event bus

## Running the API
To run the API you need to do the following:

1. Run the [EventBus](./EventBus/README.md).

2. Open up Visual Studio and open the solution `Api\Api.sln`.  Under the start-up projects select `docker-compose` and press the play button. It should open up a swagger API page that you can use to run HTTP REST commands.

## Code Evaluation
Code is evaluated by both using [Unit Tests](./AppTests/README.md#unit-tests) and [Test Coverage](./AppTests/README.md#test-coverage).  See the [guidelines](./AppTests/README.md#guidelines) for details.