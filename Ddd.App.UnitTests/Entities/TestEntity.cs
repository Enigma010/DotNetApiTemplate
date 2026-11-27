using Ddd.App.Core;
using Ddd.App.Repositories.Dtos;
using Ddd.App.UnitTests.Repositories.Dtos;

namespace AppTests.Entities
{
    public class TestEntity : Entity<TestEntityDto, Guid>
    {
        public TestEntity(TestEntityDto dto) : base(dto)
        {
        }
        public TestEntity() : base(Guid.NewGuid) { }

        public string Name => _dto.Name;
        public void ReName(string name)
        {
            _dto.Name = name;
        }
        public override void Deleted()
        {
        }
    }
}
