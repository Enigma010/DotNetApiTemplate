using App.Entities;
using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTests.Entities
{
    public class TestEntity : Entity<TestEntityDto, Guid>
    {
        public TestEntity() : base(Guid.NewGuid) { }
    }

    public class TestEntityDto : EntityDto<Guid>
    {
        public TestEntityDto(Func<Guid> getNewId) : base(getNewId)
        {
        }
    }
}
