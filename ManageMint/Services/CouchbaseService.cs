using Couchbase;
using ManageMint.Models;
using Microsoft.Extensions.Options;

namespace ManageMint.Services;

public class CouchbaseService : IDatabaseService
{
    private readonly IOptions<Models.Configuration.Couchbase> config;
    private readonly ClusterOptions clusterOptions;
    
    public CouchbaseService(IOptions<Models.Configuration.Couchbase> config)
    {
        this.config = config;
        clusterOptions = new ClusterOptions()
        {
            UserName = config.Value.Username,
            Password = config.Value.Password
        };
    }
    
    public async Task<List<Person>> GetPeopleAsync()
    {
        //var cluster = await Cluster.ConnectAsync(config.Value.ConnectionString, clusterOptions);
        throw new NotImplementedException();
    }
}