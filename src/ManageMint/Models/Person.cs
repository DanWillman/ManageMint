using Realms;

namespace ManageMint.Models;

public partial class Person : RealmObject
{
    /// <summary>
    /// Mongo generated Id
    /// </summary>
    [PrimaryKey]
    public Guid Id { get; set; }
    
    /// <summary>
    /// <see cref="Person.Id"/> of this Person's Manager
    /// </summary>
    public Guid ManagerId { get; set; }
    
    /// <summary>
    /// Collection of <see cref="Person.Id"/>s of this Person's direct reports
    /// </summary>
    public IList<Guid> ReportsIds { get; }
    
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

    public IList<Guid> DiaryEntries { get; }
    
    public IList<Person> VersionHistory { get; }
    public int Version { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}