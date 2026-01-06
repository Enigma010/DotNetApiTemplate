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

## Setup
### Windows Subsystem for Linux (WSL) Setup
A number of scripts written for this project use unix shells for ease of distribution and running.  To get these working locally you will need set this up.  Start by installing Ubuntu from the windows store. Once you've installed Ubuntu you need to set it as the default [wsl](https://learn.microsoft.com/en-us/windows/wsl/) distribution.  To do that use the following command to get a list of the distributions:

```
wsl -l
```

You'll see something like this:

```
Ubuntu-20.04 (Default)
docker-desktop-data
docker-desktop
```

From there find the entry for Ubuntu, in the above example it's `Ubuntu-20.04` once you have this use the value with the `wsl -s` to set the default distribution to Ubuntu, as an example from the above:

```
wsl -s Ubuntu-20.04
```

### Docker Installation/Setup
Docker is used for virtualization and ease of deployment.  You will need to install docker for this project to work.  For windows you can follow the steps [here](https://docs.docker.com/desktop/setup/install/windows-install/).

Next you'll need to create/modify the `Api\.env` file.  This file is a Docker [.env](https://docs.docker.com/reference/compose-file/services/#env_file) file.  Set the following values in the file:

| Property | Description | Notes |
| - | - | - |
| **APP_DOMAIN** | The name of the domain of the project | All lower case. |
| **APP_SUBDOMAIN** | The name of the sobdomai of the project | All lower case. |
| **APP_DB_USERNAME** | The username to connect to the database as | |
| **APP_DB_PASSWORD** | The password to connect to the database with | |
| **EVENT_BUS_HOST** | The host were the event bus is | In development this will be **host.docker.internal** to connect to the container running the EventBus |
| **EVENT_BUS_USERNAME** | The username to connect to the event bus with | Default is **guest** |
| **EVENT_BUS_PASSWORD** | The password to connect to the event bus with | Default is **guest** |
| **EVENT_BUS_PORT** | The port to use to connect to the event bus | Default is **5672** |
| **EVENT_BUS_HOST** | The host were the event bus is | In development this will be **host.docker.internal** to connect to the container running the EventBus |
| **GITHUBCFG_USERNAME** | The GitHub username for nuget package retrieval |
| **GITHUBCFG_PAT** | The GitHub PAT (personal access token) for nuget package retrieval |
| **GITHUBCFG_NAMESPACE** | The GitHub namespace for nuget package retrieval | For personal GitHub accounts the same as the **GITHUBCFG_USERNAME** |

There are setups for both the [EventBus](./EventBus/README.md#setup) and the [MongoDB](./Db/README.md#setup), review both of these and complete any necessary steps.

## Running the API
To run the API you need to do the following:

1. Run the [EventBus](./EventBus/README.md).

2. Open up Visual Studio and open the solution `Api\Api.sln`.  Under the start-up projects select `docker-compose` and press the play button. It should open up a swagger API page that you can use to run HTTP REST commands.

## Code Evaluation
Code is evaluated by both using [Unit Tests](./AppTests/README.md#unit-tests) and [Test Coverage](./AppTests/README.md#test-coverage).  See the [guidelines](./AppTests/README.md#guidelines) for details.