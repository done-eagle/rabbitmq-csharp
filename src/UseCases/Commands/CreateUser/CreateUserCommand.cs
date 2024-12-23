using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateUser;

public class CreateUserCommand : IRequest<int>
{
    public CreateUserDto Dto { get; set; }
}