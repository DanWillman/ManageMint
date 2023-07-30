using ManageMint.Models;

namespace ManageMint.DataAccess;

public interface IMongoService
{
    public Person GetManager(Guid reportId);

    public List<Person> GetReports(Guid managerId);

    public List<Person> GetAllPersons();
}