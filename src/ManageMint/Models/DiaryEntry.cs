using Realms;

namespace ManageMint.Models;

public class DiaryEntry : RealmObject
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    
    public string Title { get; set; }
    
    public string Body { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset UpdatedAt { get; set; }

    public override string ToString()
    {
        return Title;
    }
}