using System.Diagnostics.CodeAnalysis;
using AutoBogus;
using FluentAssertions;
using ManageMint.DataAccess;
using MongoDB.Driver;
using ManageMint.Models;
using ManageMint.Models.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Testcontainers.MongoDb;

namespace ManageMint.UnitTests;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class MongoTests
{
    private const string COLLECTION_NAME = "people";
    private IMongoDatabase db;
    private List<Person> fakes;

    private AutoFaker<Person> personFaker;
    private MongoDbContainer mongoContainer;

    [OneTimeSetUp]
    public async Task Setup()
    {
        mongoContainer = new MongoDbBuilder()
            .WithUsername("local")
            .WithPassword("local")
            .Build();
        await mongoContainer.StartAsync();
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        // This and the above serializer settings are what keeps guids in mongo in c# format, not their weird bindata format.
        // This will be default in the next version of the driver, allegedly, which is why it's flagged obsolete. https://www.mongodb.com/community/forums/t/c-guid-style-dont-work/126901/2
        // When removed from driver, this should be safe to remove.
#pragma warning disable CS0618
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
        
        var client = new MongoClient(mongoContainer.GetConnectionString());
        db = client.GetDatabase("testing");
        var personCollection = db.GetCollection<Person>(COLLECTION_NAME);
        
        if (await personCollection.EstimatedDocumentCountAsync() == 0)
            await db.CreateCollectionAsync(COLLECTION_NAME);
        
        personFaker = new AutoFaker<Person>();

        fakes = personFaker.Generate(10);
        
        await personCollection.InsertManyAsync(fakes);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await mongoContainer.StopAsync();
    }

    [Test]
    public async Task MongoService_GetAllPersons_ReturnsExpectedCollection()
    {
        var sut = new MongoService(db, Options.Create(new Mongo()
        {
            PersonCollectionName = COLLECTION_NAME
        }));

        var actualReports = sut.GetAllPersons();

        actualReports.Should().OnlyHaveUniqueItems(r => r.Id).And.HaveCount(fakes.Count)
            .And.BeEquivalentTo(fakes, opt => opt.Including(p => p.Id), "They should be the same collection");
    }

    [Test]
    public async Task MongoService_GetReports_ReturnsMatchingCollection()
    {
        var personCollection = db.GetCollection<Person>(COLLECTION_NAME);

        var sut = new MongoService(db, Options.Create(new Mongo()
        {
            PersonCollectionName = COLLECTION_NAME
        }));
        
        var fakeReportsManagerId = Guid.NewGuid();
        var fakeReports = personFaker.RuleFor(p => p.ManagerId, _ => fakeReportsManagerId).Generate(3);
        
        await personCollection.InsertManyAsync(fakeReports);
        
        var actualReports = sut.GetReports(fakeReportsManagerId);

        actualReports.Should().OnlyHaveUniqueItems(r => r.Id).And.HaveCount(fakeReports.Count)
            .And.BeEquivalentTo(fakeReports, opt => opt.Including(p => p.Id), "They should be the same collection");
    }
}