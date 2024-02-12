using RabbitSampleApis.Consumer.Model;

namespace RabbitSampleApis.Consumer.Repository;

public interface IUserRepository
{
    void AddUser(string message);
    User? GetUserById(string message);
    void UpdateUser(string message);
    void DeleteUser(string message);
}