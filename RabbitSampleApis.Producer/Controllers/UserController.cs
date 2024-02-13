using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitSampleApis.Helper.Dto;
using RabbitSampleApis.SharedModels;

namespace RabbitSampleApis.Producer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UserController> _logger;

    public UserController(IPublishEndpoint publishEndpoint, ILogger<UserController> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] AddUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        await _publishEndpoint.Publish<IUserCreated>(new
        {
            userDto.Name,
            userDto.Email
        });
        
        return Ok();
    }
    
    [HttpGet]
    public async Task <ActionResult> GetUserById([FromBody] GetUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        await _publishEndpoint.Publish<IUserReceived>(new
        {
            userDto.Id
        });
        
        return Ok(userDto);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        await _publishEndpoint.Publish<IUserUpdated>(new
        {
            userDto.Id,
            userDto.Name,
            userDto.Email
        });
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<ActionResult> DeleteUser([FromBody] DeleteUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        await _publishEndpoint.Publish<IUserDeleted>(new
        {
            userDto.Id
        });
        
        return Ok();
    }
}