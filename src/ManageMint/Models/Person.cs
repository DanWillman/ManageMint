using Realms;

namespace ManageMint.Models;

public partial class Person : RealmObject
{
    /// <summary>
    /// Mongo generated Id
    /// </summary>
    [PrimaryKey]
    public Guid Id { get; set; }
    
    public string Role { get; set; }
    
    /// <summary>
    /// Individual's preferred Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Date individual joined the company
    /// </summary>
    public DateTimeOffset HireDate { get; set; }
    
    /// <summary>
    /// Individual's birthday
    /// </summary>
    public DateTimeOffset Birthday { get; set; }
    
    /// <summary>
    /// Free form text notes, supports markdown 
    /// </summary>
    public string Notes { get; set; }
    
    public IList<Person> VersionHistory { get; }
    public int Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public override string ToString()
    {
        return Name;
    }
}