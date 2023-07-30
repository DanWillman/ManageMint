namespace ManageMint.Models.Configuration;

public class Mongo
{
    public string ConnectionString { get; set; }
    public string PersonCollectionName { get; set; } = "People";
    public string Database { get; set; } = "ManageMint";
}