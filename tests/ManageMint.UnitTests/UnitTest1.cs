namespace ManageMint.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.System));
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        Assert.Pass();
    }
}