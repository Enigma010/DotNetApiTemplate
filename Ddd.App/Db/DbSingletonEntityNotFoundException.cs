using System;
using System.Collections.Generic;
using System.Text;

namespace Ddd.App.Db
{
    /// <summary>
    /// Represents the exception that is thrown when a singleton entity of a specified type cannot be found in the
    /// database.
    /// </summary>
    /// <remarks>This exception is typically thrown when an operation expects a unique instance of an entity
    /// type to exist in the database, but no such entity is found. The EntityType property identifies the type of the
    /// missing entity, which can be used for error handling or logging purposes.</remarks>
    public class DbSingletonEntityNotFoundException : Exception
    {
        public DbSingletonEntityNotFoundException(Type entityType) 
        { 
            EntityType = entityType;
        }
        public Type EntityType
        {
            get;
            private set;
        }
    }
}
