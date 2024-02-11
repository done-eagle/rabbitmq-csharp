using Microsoft.AspNetCore.Mvc;
using RabbitSampleApis.Helper.Dto;
using RabbitSampleApis.Helper.RabbitMq;

namespace RabbitSampleApis.Producer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IRabbitMqService _mqService;
    private readonly ILogger<UserController> _logger;
    
    public UserController(ILogger<UserController> logger, IRabbitMqService mqService)
    {
        _logger = logger;
        _mqService = mqService;
    }
    
    [HttpPost]
    public ActionResult AddUser([FromBody] AddUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        _mqService.SendMessage(userDto);
        return Ok();
    }
    
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid request data");
        var userDto = new GetUserRequestDto(id);
        
        _mqService.SendMessage(userDto);
        return Ok(userDto);
    }
    
    [HttpPut]
    public IActionResult UpdateUser([FromBody] UpdateUserRequestDto? userDto)
    {
        if (userDto == null)
            return BadRequest("Invalid request data");
        
        _mqService.SendMessage(userDto);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid request data");
        var userDto = new DeleteUserRequestDto(id);
        
        _mqService.SendMessage(userDto);
        return Ok();
    }
}