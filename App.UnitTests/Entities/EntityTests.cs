using App.Core;

namespace AppTests.Entities
{
    public class EntityTests
    {
        [Fact]
        public void NonOverriddenDeleteThrows()
        {
            Entity<EntityTestsDto, Guid> entity = new Entity<EntityTestsDto, Guid>(Guid.NewGuid);
            Assert.Throws<InvalidOperationException>(() => entity.Deleted());
        }

        public class EntityTestsDto : EntityDto<Guid>
        {
            public EntityTestsDto(Func<Guid> getNewId) : base(getNewId)
            {
            }
        }
    }
}
