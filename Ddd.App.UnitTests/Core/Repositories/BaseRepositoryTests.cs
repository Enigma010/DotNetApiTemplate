using AppTests.Entities;
using AppTests.Repositories;
using Ddd.App.Core.Repositories;
using Ddd.App.Db;
using Ddd.App.UnitTests.Repositories.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using System.Linq.Expressions;

namespace Ddd.App.UnitTests.Core.Repositories
{
    public class BaseRepositoryTests
    {
        private readonly BaseRepository<ITestEntityRepository, TestEntity, TestEntityDto, Guid> _repository;
        private Mock<IDbClient> _client;
        private Mock<ILogger<ITestEntityRepository>> _logger;
        public BaseRepositoryTests()
        {
            _client = new Mock<IDbClient>();
            _logger = new Mock<ILogger<ITestEntityRepository>>();
            _repository = new TestEntityRepository(_client.Object, _logger.Object);
        }
        [Fact]
        public async Task InsertAsync()
        {
            TestEntity entity = new TestEntity();
            await _repository.Begin();
            TestEntity changeEntity = await _repository.InsertAsync(entity);
            await _repository.Commit();
            Assert.Equal(entity, changeEntity);
            Assert.Empty(changeEntity.GetEvents());
            _client.Verify(x => x.Begin(), Times.Once);
            _client.Verify(x => x.Commit(), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            TestEntity entity = new TestEntity();
            TestEntity changeEntity = await _repository.UpdateAsync(entity);
            Assert.Equal(entity, changeEntity);
            Assert.Empty(changeEntity.GetEvents());
        }
        [Fact]
        public async Task DeleteAsync()
        {
            TestEntity entity = new TestEntity();
            await _repository.DeleteAsync(entity);
            Assert.Empty(entity.GetEvents());
        }
        [Fact]
        public  async Task GetAsyncById()
        {
            TestEntity entity = new TestEntity();
            _client.Setup(x => x.GetAsync<TestEntityDto, Guid>(It.Is<Guid>(id => id == entity.Id)))
                .ReturnsAsync(entity.GetDto());
            TestEntity checkEntity = await _repository.GetAsync(entity.Id);
            Assert.Equal(entity.Id, checkEntity.Id);
            Assert.Equal(entity.GetDto(), checkEntity.GetDto());
        }
        [Fact]
        public async Task GetAsyncByExpression()       
        {
            TestEntity entity = new TestEntity();
            _client.Setup(x => x.GetAsync<TestEntityDto, Guid>(
                It.IsAny<Expression<Func<TestEntityDto, bool>>>(),
                It.IsAny<Paging>(),
                It.IsAny<Expression<Func<TestEntityDto, object>>?>()))
                .ReturnsAsync(new List<TestEntityDto>() { entity.GetDto() });
            IEnumerable<TestEntity> checkEntities = await _repository.GetAsync(x => x.Id == entity.Id, new Paging());
            Assert.Single(checkEntities);
            TestEntity checkEntity = checkEntities.First();
            Assert.Equal(entity.Id, checkEntity.Id);
            Assert.Equal(entity.GetDto(), checkEntity.GetDto());
        }
        [Fact]
        public async Task GetAsyncByPaging()
        {
            List<TestEntity> entities = new List<TestEntity>()
            {
                new TestEntity(),
                new TestEntity(),
                new TestEntity()
            };
            _client.Setup(x => x.GetAsync<TestEntityDto, Guid>(
                It.IsAny<Paging>(),
                It.IsAny<Expression<Func<TestEntityDto, object>>?>()))
                .ReturnsAsync(entities.Select(e => e.GetDto()).ToList());
            IEnumerable<TestEntity> checkEntities = await _repository.GetAsync(new Paging());
            entities.ForEach(entity =>
            {
                TestEntity? checkEntity = checkEntities.FirstOrDefault(e => e.Id == entity.Id);
                Assert.NotNull(checkEntity);
                Assert.Equal(entity.GetDto(), checkEntity.GetDto());
            });
        }
        [Fact]
        public async Task RollbackAsync()
        {
            TestEntity entity = new TestEntity();
            _client.Setup(x => x.InsertAsync<TestEntityDto, Guid>(It.IsAny<TestEntityDto>()))
                .ThrowsAsync(new InvalidOperationException());
            try
            {
                await _repository.Begin();
                await _repository.InsertAsync(entity);
                await _repository.Commit();
            }
            catch(Exception ex)
            {
                await _repository.Rollback();
                Assert.IsType<InvalidOperationException>(ex);
            };
            _client.Verify(x => x.Begin(), Times.Once);
            _client.Verify(x => x.Commit(), Times.Never);
            _client.Verify(x => x.Rollback(), Times.Once);
        }
    }
}
