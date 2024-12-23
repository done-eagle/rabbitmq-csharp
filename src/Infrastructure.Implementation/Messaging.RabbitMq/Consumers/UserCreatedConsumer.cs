using AutoMapper;
using DataAccess.PostgreSql;
using Entities;
using MassTransit;
using UseCases.Dto;

namespace Messaging.RabbitMq.Consumers;

public class UserCreatedConsumer : IConsumer<CreateUserDto>
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;

    public UserCreatedConsumer(IMapper mapper, AppDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CreateUserDto> consumeContext)
    {
        var userCreated = consumeContext.Message;
        var user = _mapper.Map<User>(userCreated);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}