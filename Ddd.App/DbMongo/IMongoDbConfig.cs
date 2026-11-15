namespace Ddd.App.DbMongo
{
    public interface IMongoDbConfig
    {
        string Password { get; set; }
        string Username { get; set; }
    }
}