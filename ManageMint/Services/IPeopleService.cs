using ManageMint.Models;

namespace ManageMint.Services;

public interface IPeopleService
{
    public Task<List<Person>> GetAllPeopleAsync();

    public Task<List<Person>> GetReportsAsync(Guid managerId);

    public Task<Person> GetManagerAsync(Guid reportId);
}