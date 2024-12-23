using DataAccess.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.PostgreSql;

public class AppDbContext : DbContext, IDbContext
{
    public DbSet<User> Users { get; set; }
    private readonly string _connectionString;

    public AppDbContext()
    {
    }
    
    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=InfoUser; Username=postgres; Password=admin");
}