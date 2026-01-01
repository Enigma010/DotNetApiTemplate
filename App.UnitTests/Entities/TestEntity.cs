using App.Core;

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
