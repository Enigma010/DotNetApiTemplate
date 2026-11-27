using Ddd.App.Core;

namespace Ddd.App.UnitTests.Repositories.Dtos
{
    public class TestEntityDto : EntityDto<Guid>
    {
        public TestEntityDto(Func<Guid> getNewId) : base(getNewId)
        {
        }

        public string Name { get; set; } = string.Empty;
    }
}
