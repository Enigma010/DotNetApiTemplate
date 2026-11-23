namespace Ddd.App.Core.Repositories
{
    public interface IUpdatableRepository<EntityType, EntityDtoType, IdType>
        where EntityDtoType : EntityDto<IdType>
        where EntityType : Entity<EntityDtoType, IdType>
        where IdType : IComparable
    {
        Task<EntityType> UpdateAsync(EntityType entity);
    }
}
