using Microsoft.EntityFrameworkCore;

namespace CarActiveRecord;

public class CarDbContext : DbContext
{

    public DbSet<Car> Cars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=51170;Database=docker;Username=docker;Password=docker");
    }
}
