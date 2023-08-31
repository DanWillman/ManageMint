using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Realms;

namespace ManageMint.DataAccess;

public class MongoService : IMongoService
{
    private readonly Realm realm;

    public MongoService(IOptions<Mongo> options)
    {
        realm = Realm.GetInstance(options.Value.DatabasePath);
    }

    public List<Person> GetReports(Guid managerId)
    {
        return realm.All<Person>().Where(p => p.ManagerId.Equals(managerId)).ToList();
    }

    public List<Person> GetAllPersons()
    {
        return realm.All<Person>().ToList();
    }

    public Person GetPerson(Guid id)
    {
        return realm.All<Person>().FirstOrDefault(p => p.Id.Equals(id));
    }
}