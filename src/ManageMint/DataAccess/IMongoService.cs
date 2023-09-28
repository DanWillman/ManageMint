using ManageMint.Models;

namespace ManageMint.DataAccess;

public interface IMongoService
{
    public Person GetPerson(Guid id);

    public List<Person> GetReports();

    public List<Person> GetAllPersons();
    void UpsertDiaryEntry(DiaryEntry entry, string body);
    List<DiaryEntry> GetDiaryEntries();
    void DeleteDiaryEntry(Guid id);
    void GetDiaryEntry(Guid id);
    List<Person> GetManagers();
    void UpsertPerson(Person person, string name = null, string notes = null, DateTime? birthday = null, DateTime? hireDate = null);
    void DeletePerson(Guid id);
}