using ManageMint.Models;

namespace ManageMint.DataAccess;

public interface IMongoService
{
    public Task<Person> GetManager(Guid reportId);
}