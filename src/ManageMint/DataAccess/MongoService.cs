using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ManageMint.DataAccess;

public class MongoService : IMongoService
{
    private readonly IMongoCollection<Person> personCollection;

    public MongoService(IMongoDatabase mongoDb, IOptions<Mongo> options)
    {
        personCollection = mongoDb.GetCollection<Person>(options.Value.PersonCollectionName);
    }

    public List<Person> GetReports(Guid managerId)
    {
        return personCollection.Find(p => p.ManagerId.Equals(managerId)).ToList();
    }

    public List<Person> GetAllPersons()
    {
        return personCollection.Find(p => true).ToList();
    }

    public Person GetManager(Guid reportId)
    {
        return personCollection.Find(p => p.Id.Equals(reportId)).FirstOrDefault();
    }
}