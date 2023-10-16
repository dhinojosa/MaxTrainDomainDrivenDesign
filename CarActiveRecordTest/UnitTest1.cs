using CarActiveRecord;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace CarActiveRecordTest;

[TestFixture]
public class DbTest
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithPortBinding(51170, 5432)
        .WithExposedPort(51170)
        //.WithResourceMapping(@"Resources\import.sql", "/docker-entrypoint-initdb.d/")
        .WithCommand()
        .Build();

    [OneTimeSetUp]
    public void Setup()
    {
        Console.WriteLine(Environment.CurrentDirectory);
        _postgres.StartAsync().GetAwaiter().GetResult();
        var connectionString = _postgres.GetConnectionString();
        Console.WriteLine(connectionString);

    }

    [Test]
    public void Test1()
    {

        // var car = new Car("Toyota", "Camry", 2022, 25000.00M);
        // car.Save();
        //
        // Console.WriteLine(car.Id);
        //
        // // Find a car by its ID
        // var retrievedCar = Car.Find(1);
        // Console.WriteLine($"Found car: {retrievedCar.Make} {retrievedCar.Model} ({retrievedCar.Year}), Price: {retrievedCar.Price:C}");
        //
        // // Delete a car
        // car.Delete();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _postgres.StopAsync().GetAwaiter().GetResult();
        _postgres.DisposeAsync().GetAwaiter().GetResult();
    }
}
