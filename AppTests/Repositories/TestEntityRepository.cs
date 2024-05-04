using App.Repositories;
using AppTests.Entities;
using Db;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTests.Repositories
{
    public interface ITestEntityRepository : IBaseRepository<TestEntity, Guid>
    {
    }
    public class TestEntityRepository : BaseRepository<ITestEntityRepository, TestEntity, Guid>
    {
        public TestEntityRepository(IDbClient client, ILogger<ITestEntityRepository> logger) : base(client,logger)
        {
        }
    }
}
