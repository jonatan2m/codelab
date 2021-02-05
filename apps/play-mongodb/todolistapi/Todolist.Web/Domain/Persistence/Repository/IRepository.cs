using MongoDB.Driver;

namespace Todolist.Web.Domain.Persistence.Repository
{
    public interface IRepository<T>
    {
         IMongoCollection<T> Collection {get;}
         IMongoDatabase Database {get;}
    }
}