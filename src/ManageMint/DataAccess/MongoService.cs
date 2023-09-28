using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Platform;
using Realms;

namespace ManageMint.DataAccess;

public class MongoService : IMongoService
{
    private readonly Realm realm;

    public MongoService(IOptions<Mongo> options)
    {
        var config = new RealmConfiguration(options.Value.DatabasePath)
        {
            SchemaVersion = 2,
            MigrationCallback = (migration, oldVersion) =>
            {
                var oldReports = migration.OldRealm.DynamicApi.All(nameof(Person));
                var newReports = migration.NewRealm.All<Person>();

                for (var i = 0; i < oldReports.Count(); i++)
                {
                    var previousReport = oldReports.ElementAt(i);
                    var newReport = newReports.ElementAt(i);
                    
                    // Schema 1 => 2
                    if (oldVersion < 2)
                    {
                        var previousRole = previousReport.DynamicApi.Get<string>(nameof(Person.Role));
                        newReport.Role = string.IsNullOrWhiteSpace(previousRole) ? string.Empty : previousRole;
                    }
                    
                }
            }
        };
        
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

    public List<Person> GetReports()
    {
        try
        {
            return realm.All<Person>().Where(p => p.Role.Equals(Role.DirectReport)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<Person> GetManagers()
    {
        try
        {
            return realm.All<Person>().Where(p => p.Role.Equals(Role.Manager)).ToList();
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
    
    public void UpsertPerson(Person person, string name = null, string notes = null, DateTime? birthday = null, DateTime? hireDate = null)
    {
        try
        {
            realm.Write(() =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                    person.Name = name;
                if (!string.IsNullOrWhiteSpace(notes))
                    person.Notes = notes;
                if (birthday != null)
                    person.Birthday = new DateTimeOffset(birthday.Value);
                if (hireDate != null)
                    person.HireDate = new DateTimeOffset(hireDate.Value);
                person.UpdatedAt = DateTimeOffset.Now;
                person.Version++;
                person.VersionHistory.Add(person);
                realm.Add(person, true);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void DeletePerson(Guid id)
    {
        try
        {
            var person = realm.All<Person>().FirstOrDefault(e => e.Id == id);
            realm.Write(() =>
            {
                realm.Remove(person);
            });
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
            var entry = realm.All<DiaryEntry>().FirstOrDefault(e => e.Id == id);
            realm.Write(() =>
            {
                if (entry != null)
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