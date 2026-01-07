# API
Includes objects for interacting from a HTTP REST API.

## Guidelines
The following describes guidelines for the architecture.

* The API takes in HTTP requests and translates them into objects and method calls for interacting with the core business logic in the service layer.

HTTP Verbs

| Verb | Definition |
| - | - |
| GET | Retrieves a representation of the specified resource or collection of resources. This method is safe (doesn't alter server state) and cacheable. |
| POST| Used to create a new resource. The data for the new resource is included in the body of the request. POST requests are not idempotent, meaning multiple identical requests can result in multiple new resources being created. |
| PUT | Updates or replaces an entire existing resource with the data provided in the request body. If the resource does not exist at the specified URI, PUT can be used to create it. This method is idempotent, meaning multiple identical PUT requests will have the same effect as a single one. |
| PATCH | Applies partial modifications to a resource. It only updates specific fields provided in the request body, unlike PUT, which replaces the entire resource. |
| DELETE | Removes the specified resource from the server. |
