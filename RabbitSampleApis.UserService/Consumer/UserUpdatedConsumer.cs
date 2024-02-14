using AutoMapper;
using MassTransit;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.UserService.Model;
using RabbitSampleApis.UserService.Repository;

namespace RabbitSampleApis.UserService.Consumer;

public class UserUpdatedConsumer : IConsumer<IUserUpdated>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserUpdatedConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IUserUpdated> context)
    {
        var userUpdated = context.Message;
        var user = _mapper.Map<User>(userUpdated);
        await _userRepository.UpdateUserAsync(user);
    }
}