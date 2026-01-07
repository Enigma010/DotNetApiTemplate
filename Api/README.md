# API
Includes objects for interacting from a HTTP REST API.

## Guidelines
The following describes guidelines for the architecture.

* The API takes in HTTP requests and translates them into objects and method calls for interacting with the core business logic in the service layer.
* The underlying API will utilize standard REST concepts of being resource based while the underlying infrastructure will be largely CQRS based. As such commands will need to be translated to resource nouns for representation in the REST APIs.
* All query based operations of the CQRS definitions will be represented by the REST resource **GET** verb.
* All command based operations of the CQRS defintions will be represented by the REST resource **POST** verb.
* All commands based on removing an aggregate, entity, or some data will be represented by the REST resource **DELETE** verb.
* REST resource **PUT**/**PATCH** verbs will not be mapped to any specific meaning and as such should not be used.
