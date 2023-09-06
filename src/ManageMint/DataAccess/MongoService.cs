using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using Realms;

namespace ManageMint.DataAccess;

public class MongoService : IMongoService
{
    private readonly Realm realm;

    public MongoService(IOptions<Mongo> options)
    {
        var config = new RealmConfiguration(options.Value.DatabasePath);
        try
        {
            if (!File.Exists(options.Value.DatabasePath)
                && !Directory.Exists(Path.GetDirectoryName(options.Value.DatabasePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(options.Value.DatabasePath));
            }

            realm ??= Realm.GetInstance(config);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<Person> GetReports(Guid managerId)
    {
        try
        {
            return realm.All<Person>().Where(p => p.ManagerId.Equals(managerId)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<Person> GetAllPersons()
    {
        try
        {
            return realm.All<Person>().ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Person GetPerson(Guid id)
    {
        try
        {
            return realm.All<Person>().FirstOrDefault(p => p.Id.Equals(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void UpsertDiaryEntry(DiaryEntry entry, string body)
    {
        try
        {
            realm.Write(() =>
            {
                entry.UpdatedAt = DateTimeOffset.Now;
                entry.Title = entry.Body.Split(Environment.NewLine).FirstOrDefault() ?? entry.Title;
                entry.Body = body;
                realm.Add<DiaryEntry>(entry, true);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public List<DiaryEntry> GetDiaryEntries()
    {
        try
        {
            return realm.All<DiaryEntry>().ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void DeleteDiaryEntry(Guid id)
    {
        try
        {
            realm.Write(() =>
            {
                var entry = realm.All<DiaryEntry>().FirstOrDefault(e => e.Id.Equals(id));
                realm.Remove(entry);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void GetDiaryEntry(Guid id)
    {
        try
        {
            realm.All<DiaryEntry>().FirstOrDefault(e => e.Id.Equals(id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}