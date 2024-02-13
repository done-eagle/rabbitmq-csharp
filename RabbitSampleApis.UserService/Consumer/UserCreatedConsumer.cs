using AutoMapper;
using MassTransit;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.UserService.Model;
using RabbitSampleApis.UserService.Repository;

namespace RabbitSampleApis.UserService.Consumer;

public class UserCreatedConsumer : IConsumer<IUserCreated>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserCreatedConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IUserCreated> context)
    {
        var userCreated = context.Message;
        var user = _mapper.Map<User>(userCreated);
        await _userRepository.AddUserAsync(user);
    }
}