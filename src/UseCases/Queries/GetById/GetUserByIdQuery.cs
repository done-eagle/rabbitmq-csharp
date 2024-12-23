using MediatR;
using UseCases.Dto;

namespace UseCases.Queries.GetById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}