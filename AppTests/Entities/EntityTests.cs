﻿using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTests.Entities
{
    public class EntityTests
    {
        [Fact]
        public void NonOverriddenDeleteThrows()
        {
            Entity<Guid> entity = new Entity<Guid>();
            Assert.Throws<InvalidOperationException>(() => entity.Deleted());
        }
    }
}
