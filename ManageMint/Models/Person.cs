namespace ManageMint.Models;

public class Person
{
    public Guid Id { get; set; }
    
    public Guid ManagerId { get; set; }
    
    public List<Guid> ReportsIds { get; set; }
    
    public DateTime HireDate { get; set; }
    
    public DateTime Birthday { get; set; }
    
    public string Notes { get; set; }

    public List<Guid> DiaryEntries { get; set; }
}