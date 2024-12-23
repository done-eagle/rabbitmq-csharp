using AutoMapper;
using DataAccess.PostgreSql;
using MassTransit;
using UseCases.Dto;

namespace Messaging.RabbitMq.Consumers;

public class UserReceivedConsumer : IConsumer<UserDto>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;

    public UserReceivedConsumer(IPublishEndpoint publishEndpoint, IMapper mapper, AppDbContext dbContext)
    {
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserDto> context)
    {
        var userReceived = context.Message;
        var user = await _dbContext.Users.FindAsync(userReceived.Id);;
        var userDto = _mapper.Map<UserDto>(user);
        await context.RespondAsync(userDto);
    }
}