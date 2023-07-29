using ManageMint.Models;

namespace ManageMint.Services;

public interface IDatabaseService
{
    public Task<List<Person>> GetPeopleAsync();
}