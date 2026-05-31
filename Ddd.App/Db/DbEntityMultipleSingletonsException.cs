namespace Ddd.App.Db
{
    public class DbEntityMultipleSingletonsException<EntityType> : Exception
    {
        public DbEntityMultipleSingletonsException(EntityType existingEntity) : base()
        {
            ExistingEntity = existingEntity;
        }
        public EntityType ExistingEntity
        {
            get;
            private set;
        }
    }
}
