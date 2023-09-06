namespace ManageMint.Models.Configuration;

public class Mongo
{
    public string PersonCollectionName { get; set; } = "People";
    public string RealmName { get; set; } = "ManageMint";
    public string DatabasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ManageMint", "db.realm");
}