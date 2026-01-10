# API
Includes objects for interacting from a HTTP REST API.

## Guidelines
The following describes guidelines for the architecture.

* The API will utilize standard REST concepts of being resource based while the underlying infrastructure will be CQRS based. As such commands will need to be translated to resource nouns for representation in the REST APIs.
* All REST API nouns should be plural, unless there can be only one of the resoruce and then they can be singular.
* All query based operations of the CQRS definitions will be represented by the REST resource **GET** verb.
* All command based operations of the CQRS defintions will be represented by the REST resource **POST** verb.
* All commands based on removing an aggregate, entity, or some data will be represented by the REST resource **DELETE** verb.
* REST resource **PUT**/**PATCH** verbs will not be mapped to any specific meaning and as such should not be used.
* All routes will be in lowercase.
* Words will be separated by hyphens like `word-one`.
* All REST API nouns should use the their base end point to have a paged list of those entities example `/configs`.  This end point should allow optionally one to many search terms to be applied to search through the list of entities returned.
* All REST API root level nouns should then use their internal identifier for the next part of the path, example `/configs/{id}`.  At this level you should have a **GET** verb that retrieves the whole entity and a **DELETE** verb to delete the entity (if deletes can be are allowed).
* Past the internal identifier you will see resources, in the form of nouns, for that entity that correspond to commands in the CQRS resources.  Example: **POST** `/configs/{id}/name` will run the `ConfigService.RenameAsync` command
* 

Examples
| API Verb | API Endpoint | CQRS Entity | CQRS Command | Description |
| - | - | - | - | - |
| **POST** | **/configs** | Config | `ConfigService.CreateAsync(cmd)` | Creates a new configuration |
| **GET** | **/configs** | Config | `ConfigService.GetAsync(paging)` | Gets a page of the configurations |
| **GET** | **/configs/{id}** | Config | `ConfigService.GetAsync(id)` | Gets a configuration |
| **DELETE** | **/configs/{id}** | Config | `ConfigService.DeleteAsync(id)` | Deletes a configuration |
| **POST** | **/configs/{id}/name** | Config | `ConfigService.RenameAsync(id, cmd)` | Renames a configuration | 
| **POST** | **/configs/{id}/enablement** | Config | `ConfigService.EnablementAsync(id, cmd)` | Changes the enable property of a configuration | 