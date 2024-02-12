using System.Text.Json;
using AutoMapper;
using RabbitSampleApis.Consumer.Context;
using RabbitSampleApis.Consumer.Model;
using RabbitSampleApis.Helper.Dto;

namespace RabbitSampleApis.Consumer.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public void AddUser(string message)
    {
        if (message.Equals(null) || message.Equals(""))
            throw new ArgumentException("Error: Message is wrong");
        
        var userDto = JsonSerializer.Deserialize<AddUserRequestDto>(message);
        
        if (userDto == null || userDto.GetType() != typeof(AddUserRequestDto))
            throw new ArgumentException("Error: Wrong type of operation");
        
        var user = _mapper.Map<User>(userDto);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User? GetUserById(string message)
    {
        if (message.Equals(null) || message.Equals(""))
            throw new ArgumentException("Error: Message is wrong");
        
        var userDto = JsonSerializer.Deserialize<GetUserRequestDto>(message);
        
        if (userDto == null || userDto.GetType() != typeof(GetUserRequestDto))
            throw new ArgumentException("Error: Wrong type of operation");
        
        var user = _context.Users.Find(userDto.Id);
        return user;
    }

    public void UpdateUser(string message)
    {
        if (message.Equals(null) || message.Equals(""))
            throw new ArgumentException("Error: Message is wrong");
        
        var userDto = JsonSerializer.Deserialize<UpdateUserRequestDto>(message);
        
        if (userDto == null || userDto.GetType() != typeof(UpdateUserRequestDto))
            throw new ArgumentException("Error: Wrong type of operation");
        
        var user = _mapper.Map<User>(userDto);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void DeleteUser(string message)
    {
        if (message.Equals(null) || message.Equals(""))
            throw new ArgumentException("Error: Message is wrong");
        
        var userDto = JsonSerializer.Deserialize<DeleteUserRequestDto>(message);
        
        if (userDto == null || userDto.GetType() != typeof(DeleteUserRequestDto))
            throw new ArgumentException("Error: Wrong type of operation");
        
        var user = _context.Users.Find(userDto.Id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}