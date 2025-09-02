using docker_number_Minimal_API.Models;
using Microsoft.EntityFrameworkCore;


namespace docker_number_Minimal_API.Data;

public class AppDbContext : DbContext
{

    public DbSet<NumItem> NumItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}
