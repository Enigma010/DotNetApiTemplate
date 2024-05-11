﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    /// <summary>
    /// Entity interface, allows database to be able to find the ID
    /// of the entity
    /// </summary>
    /// <typeparam name="IdType">The type of ID used to identify the entity</typeparam>
    public interface IEntity<IdType>
    {
        public IdType Id { get; }
        public void Deleted();
        public void AddStateChange(object stateChanged);
        public IReadOnlyCollection<object> GetStateChanges();
        public void ClearStateChanges();
    }
}
