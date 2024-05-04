# Db
Data storage or database interfaces.  Note that for now the data storage was implemented to support the ease of development and thus wasn't unit tested.  As we have more ideas on how we want this to work this can be expanded or abandoned.

## Guidelines
The following describes guidelines for the architecture.

* This provides a contract between the repository and data storage to support changing data storage options. Don't allow repository definitions direct access to the specific data storage option.