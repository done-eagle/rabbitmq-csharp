using AutoMapper;
using MassTransit;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.SharedModels.Dto;
using RabbitSampleApis.UserService.Repository;

namespace RabbitSampleApis.UserService.Consumer;

public class UserReceivedConsumer : IConsumer<IUserReceived>
{
    private readonly IUserRepository _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public UserReceivedConsumer(IUserRepository userRepository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _userRepository = userRepository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IUserReceived> context)
    {
        var userReceived = context.Message;
        var user = await _userRepository.GetUserByIdAsync(userReceived.Id);
        var userDto = _mapper.Map<GetUserResponseDto>(user);
        await context.RespondAsync(userDto);
    }
}