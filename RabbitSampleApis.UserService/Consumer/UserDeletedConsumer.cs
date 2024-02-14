using AutoMapper;
using MassTransit;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.UserService.Model;
using RabbitSampleApis.UserService.Repository;

namespace RabbitSampleApis.UserService.Consumer;

public class UserDeletedConsumer : IConsumer<IUserDeleted>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserDeletedConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IUserDeleted> context)
    {
        var userDeleted = context.Message;
        await _userRepository.DeleteUserAsync(userDeleted.Id);
    }
}