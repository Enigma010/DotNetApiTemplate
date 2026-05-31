namespace Ddd.App.Core.Repositories
{
    public interface IGetableSingletonRepository<EntityType, EntityDtoType, IdType>
        where EntityDtoType : EntityDto<IdType>
        where EntityType : Entity<EntityDtoType, IdType>
        where IdType : IComparable
    {
        Task<EntityType> GetAsync();
    }
}
