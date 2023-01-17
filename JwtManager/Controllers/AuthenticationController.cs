using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EndPointProject.Service;
using EndPointProject.Dto;
namespace EndPointProject.Controllers;
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserService _userservice;
    private readonly JwtManager _jwtmanager;
    public AuthenticationController(UserService userservice, JwtManager jwtManager)
    {
        _userservice = userservice;
        _jwtmanager = jwtManager;
    }
    [HttpGet]
    public string Hello()
    {
        return "Hello world";
    }
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserDto usrDto)
    {
        (string username, string password) = (usrDto.Username!, usrDto.Password!);
        bool isValid = await _userservice.IsValidUser(usrDto);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var jwt = _jwtmanager.GenerateTokenjwk(username, hashedPassword);
        string json;
        if (!isValid)
        {
            json =
                JsonConvert.SerializeObject(new { message = "Not valid User" });
            return BadRequest(json);
        }
        json =
            JsonConvert.SerializeObject(new { username = username, jwt = jwt });
        return Ok(json);
    }



    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserDto usrDto)
    {
        Console.WriteLine("llegue");
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        Console.WriteLine("Estamos activo ", usrDto.Username, " y ", usrDto.Password);
        (string username, string password) = (usrDto.Username!, usrDto.Password!);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        _userservice.InsertOne(username, hashedPassword);
        var jwt = _jwtmanager.GenerateTokenjwk(username, hashedPassword);
        return Ok(jwt);
    }

}