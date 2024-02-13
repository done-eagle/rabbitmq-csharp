using Microsoft.EntityFrameworkCore;
using RabbitSampleApis.UserService.Model;

namespace RabbitSampleApis.UserService.Context;

public class AppDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public AppDbContext()
    {}
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=InfoUser; Username=postgres; Password=admin");
}