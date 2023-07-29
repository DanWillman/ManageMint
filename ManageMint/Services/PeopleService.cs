using ManageMint.Models;
using Microsoft.Extensions.Logging;

namespace ManageMint.Services;

public class PeopleService : IPeopleService
{
    private readonly ILogger<PeopleService> logger;
    private readonly IDatabaseService databaseService;
    
    public PeopleService(ILogger<PeopleService> logger, IDatabaseService databaseService)
    {
        this.logger = logger;
        this.databaseService = databaseService;
    }
    
    public Task<List<Person>> GetAllPeopleAsync()
    {
        return databaseService.GetPeopleAsync();
    }

    public Task<List<Person>> GetReportsAsync(Guid managerId)
    {
        throw new NotImplementedException();
    }

    public Task<Person> GetManagerAsync(Guid reportId)
    {
        throw new NotImplementedException();
    }
}