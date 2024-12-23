using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Commands.CreateUser;
using UseCases.Dto;
using UseCases.Queries.GetById;

namespace RabbitMqExample.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;
    
    public UserController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost("create_user")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        await _sender.Send(new CreateUserCommand {Dto = userDto});
        return Ok();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(int id)
    {
        var result = await _sender.Send(new GetUserByIdQuery {Id = id});
        return Ok(result);
    }
}