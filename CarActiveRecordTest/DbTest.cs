using CarActiveRecord;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CarActiveRecordTest;

[TestFixture]
public class DbTest
{
    private readonly PostgreSqlContainer _postgres1 = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("docker")
        .WithUsername("docker")
        .WithPassword("docker")
        .WithPortBinding(51170, 5432)
        .Build();

    [OneTimeSetUp]
    public void Setup()
    {
        Console.WriteLine(Environment.CurrentDirectory);
        _postgres1.StartAsync().GetAwaiter().GetResult();
    }

    private static async void CreateIfNotExists()
    {
        string connectionString = "Host=127.0.0.1;Port=51170;Database=docker;Username=docker;Password=docker;";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        string createTable = @"CREATE TABLE if not exists ""Cars"" (
                     ""Id"" serial PRIMARY KEY,
                     ""Make"" VARCHAR(255),
                     ""Model"" VARCHAR(255),
                     ""Price"" DECIMAL(10, 2),
                     ""Year"" INTEGER
            );";

        using (var cmd = new NpgsqlCommand(createTable, conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }
    }

    private static async void showTableNames()
    {
        string connectionString = "Host=127.0.0.1;Port=51170;Database=docker;Username=docker;Password=docker;";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        var conn = await dataSource.OpenConnectionAsync();

        string selectAll = @"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";

        using (var cmd = new NpgsqlCommand(selectAll, conn))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }
        }
    }

    [Test]
    public void TestActiveRecordActivity()
    {
        CreateIfNotExists();
        var car = new Car("Toyota", "Camry", 2022, 25000.00M);
        car.Save();

        Console.WriteLine(car.Id);

        // Find a car by its ID
        var retrievedCar = Car.Find(1);
        Console.WriteLine(
            $"Found car: {retrievedCar.Make} {retrievedCar.Model} ({retrievedCar.Year}), Price: {retrievedCar.Price:C}");

        // Delete a car
        car.Delete();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _postgres1.StopAsync().GetAwaiter().GetResult();
        _postgres1.DisposeAsync().GetAwaiter().GetResult();
    }
}
