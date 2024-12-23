using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces;

public interface IDbContext
{
    public DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken token = default);
}