# AppDi
Represents application's dependency injection.  This allows for application objects to be created without having to express their constuctors through the source code.

## Guidelines
The following describes guidelines for the architecture.

* All application dependencies should be in the **DependencyInjector.AddAppDependencies**.  Doing so will make it so the dependencies are always accurate to the caller.