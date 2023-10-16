using Npgsql;
using Testcontainers.PostgreSql;

namespace CarTransactionScriptTest;

public class CarTransactionScriptTest
{
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
            _postgres1?.StartAsync().GetAwaiter().GetResult();
        }

        [Test]
        public async Task Test1()
        {
            string connectionString = "Host=127.0.0.1;Port=51170;Database=docker;Username=docker;Password=docker;";

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            var dataSource = dataSourceBuilder.Build();

            var conn = await dataSource.OpenConnectionAsync();

            string createTable = @"CREATE TABLE car (
                     Id serial PRIMARY KEY,
                     make VARCHAR(255),
                     model VARCHAR(255),
                     price DECIMAL(10, 2),
                     year INTEGER
            );";

            await using (var cmd = new NpgsqlCommand(createTable, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            // SQL command to insert data.
            // WARNING: In production, ALWAYS use parameterized queries to prevent SQL Injection.
            string queryString = "INSERT INTO Car (make, model, price, year) VALUES (@Make, @Model, @Price, @Year)";

            await using (var cmd = new NpgsqlCommand(queryString, conn))
            {
                cmd.Parameters.AddWithValue("@Make", "Toyota");
                cmd.Parameters.AddWithValue("@Model", "Camry");
                cmd.Parameters.AddWithValue("@Price", 10000.00M);
                cmd.Parameters.AddWithValue("@Year", 2018);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _postgres1?.StopAsync().GetAwaiter().GetResult();
            _postgres1?.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
