using RabbitSampleApis.UserService.Context;
using RabbitSampleApis.UserService.Model;

namespace RabbitSampleApis.UserService.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentException("Wrong User");
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        if (id <= 0) 
            throw new ArgumentException("Wrong Id");
        
        var user = await _context.Users.FindAsync(id);
        return user;
    }
    
    public async Task UpdateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentException("Wrong User");
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Wrong Id");
        
        var user = await _context.Users.FindAsync(id);
        
        if (user != null) 
            _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}