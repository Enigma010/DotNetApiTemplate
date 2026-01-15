# Domain.SubDomain.App.Events

The Domain.SubDomain application events. Application events are emitted on the event bus where event consumers listen to the events and respond.

### Prerequisites

The application events use the [DotNetApiEventBus](https://github.com/Enigma010/DotNetApiEventBus/pkgs/nuget/DotNetApiEventBus) nuget package.

## Usage
To setup a event consumer you first need to call the `AddEventBusDependencies` to register the consumers.  As an example:

```
builder.AddEventBusDependencies(["App"]);
```

The first parameter is the list of assemblies to look for event consumers in. Event consumers are closses defined as:

```
public class CreateConfigEventConsumer : EventConsumer<ConfigCreatedEvent>
```

So they extend the `EventConsumer` class and pass in the event, in this case the `ConfigCreatedEvent` and respond to them.  All domain events for this domain are contained in this nuget package.  As an example see [CreateConfigEventConsumer.cs](https://github.com/Enigma010/DotNetApiTemplate/blob/main/AppEventConsumers/CreateConfigEventConsumer.cs).  In it's totality you can see how this works by working with the [Enigma010/DotNetApiTemplate](https://github.com/Enigma010/DotNetApiTemplate/tree/main).