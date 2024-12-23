using AutoMapper;
using DataAccess.Interfaces;
using MassTransit;
using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateUserCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish<CreateUserDto>(new
        {
            command.Dto.Name,
            command.Dto.Email
        });

        return 0;
    }
}