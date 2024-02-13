using RabbitSampleApis.UserService.Model;

namespace RabbitSampleApis.UserService.Repository;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}