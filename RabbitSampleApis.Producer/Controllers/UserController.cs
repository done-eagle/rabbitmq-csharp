using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitSampleApis.SharedModels;
using RabbitSampleApis.SharedModels.Dto;

namespace RabbitSampleApis.Producer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<IUserReceived> _getUserRequestClient;

    public UserController(IPublishEndpoint publishEndpoint, IRequestClient<IUserReceived> getUserRequestClient)
    {
        _publishEndpoint = publishEndpoint;
        _getUserRequestClient = getUserRequestClient;
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
    public async Task<ActionResult> GetUserById([FromBody] GetUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        var response = await _getUserRequestClient.GetResponse<GetUserResponseDto>(new
        {
            userDto.Id
        });
        
        return Ok(response.Message);
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