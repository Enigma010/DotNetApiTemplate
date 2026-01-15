# App.Events
Events emitted by the application.  Events are emitted through the event bus and can be subscribed to so that other parts of the application can react to events. Because events are emitted and subscribed to the event models are published as part of a nuget package.

## Guidelines
The following are guidelines related to events

* Evemts should always be emitted for commands being run.
* Events should only be emitted if the command actually takes effect, in other works the commands are idempotent.
* Event should contant the pre and post state data for an event.