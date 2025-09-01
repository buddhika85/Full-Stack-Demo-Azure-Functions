using Microsoft.EntityFrameworkCore;
using number_Minimal_API.Models;

namespace number_Minimal_API.Data;

public class AppDbContext : DbContext
{

    public DbSet<NumItem> NumItems { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}
