using AutoMapper;
using DataAccess.Interfaces;
using MassTransit;
using MediatR;
using UseCases.Commands.CreateUser;
using UseCases.Dto;

namespace UseCases.Queries.GetById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IRequestClient<UserDto> _getUserRequestClient;

    public GetUserByIdQueryHandler(IRequestClient<UserDto> getUserRequestClient)
    {
        _getUserRequestClient = getUserRequestClient;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var response = await _getUserRequestClient.GetResponse<UserDto>(new
        {
            query.Id
        });

        return response.Message;
    }
}