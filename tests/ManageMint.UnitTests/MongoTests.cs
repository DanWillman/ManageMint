using System.Diagnostics.CodeAnalysis;
using AutoBogus;
using ManageMint.DataAccess;
using MongoDB.Driver;
using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ManageMint.UnitTests;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class MongoTests
{
    private const string COLLECTION_NAME = "people";
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<Person> personCollection;
    private List<Person> fakes;

    private Guid fakeReportsManagerId;
    private List<Person> fakeReports;
    private List<Person> confused;

    [OneTimeSetUp]
    public void Setup()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        client =
            new MongoClient(
                @"mongodb+srv://dan:IiNL0wzCK6rHrWm2@managemint.onzlqof.mongodb.net/?retryWrites=true&w=majority");
        db = client.GetDatabase("testing");
        personCollection = db.GetCollection<Person>(COLLECTION_NAME);
        
        if (personCollection.EstimatedDocumentCount() == 0)
            db.CreateCollection(COLLECTION_NAME);
        
        var personFaker = new AutoFaker<Person>();

        fakes = personFaker.Generate(10);
        
        personCollection.InsertMany(fakes);

        fakeReportsManagerId = fakes[0].ManagerId;
        fakeReports = personFaker.RuleFor(p => p.ManagerId, _ => fakeReportsManagerId).Generate(3);
        
        personCollection.InsertMany(fakeReports);

        confused = personCollection.Find(p => true).ToList();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        client.DropDatabase(db.DatabaseNamespace.DatabaseName);
    }

    [Test]
    public void Test1()
    {
        var sut = new MongoService(personCollection);

        var actualReports = sut.GetReports(fakeReportsManagerId);

        Assert.That(actualReports.Count, Is.EqualTo(fakeReports.Count));
        
        foreach (var report in actualReports)
        {
            var expected = fakeReports.Find(p => p.Id.Equals(report.Id));
            
            Assert.NotNull(expected);
            
            Assert.That(report.Id, Is.EqualTo(expected?.Id));
            Assert.That(report.Name, Is.EqualTo(expected?.Name));
        }
    }
}