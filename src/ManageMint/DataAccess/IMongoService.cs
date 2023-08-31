using ManageMint.Models;

namespace ManageMint.DataAccess;

public interface IMongoService
{
    public Person GetPerson(Guid id);

    public List<Person> GetReports(Guid managerId);

    public List<Person> GetAllPersons();
}