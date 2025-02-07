using Microsoft.Extensions.Logging;
using System.Data;
using System.Linq.Expressions;
using DotNetApiUnitOfWork;

namespace Db
{
    /// <summary>
    /// Interface to a database, supports CRUD operations, used so that different database can be
    /// substituted without impacting the repository service
    /// </summary>
    public interface IDbClient : IUnitOfWork
    {
        public const string UserNamePattern = "{username}";
        public const string PasswordPattern = "{password}";
        public const string PortPattern = "{port}";
        Task InsertAsync<DbType, IdType>(DbType entity) where DbType : IDbEntity<IdType>;
        Task DeleteAsync<DbType, IdType>(DbType entity) where DbType : IDbEntity<IdType>;
        Task UpdateAsync<DbType, IdType>(DbType entity) where DbType : IDbEntity<IdType> where IdType : IComparable;
        Task<DbType> GetAsync<DbType, IdType>(IdType id);
        Task<IEnumerable<DbType>> GetAsync<DbType, IdType>(Paging paging, Expression<Func<DbType, object>>? sort = null) where DbType : IDbEntity<IdType>;
        Task<IEnumerable<DbType>> GetAsync<DbType, IdType>(Expression<Func<DbType, bool>> expression, Paging paging, Expression<Func<DbType, object>>? sort = null) where DbType : IDbEntity<IdType>;
    }
}
