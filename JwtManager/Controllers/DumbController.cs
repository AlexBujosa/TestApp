namespace EndPointProject.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using EndPointProject.Service;
[Route("api/[controller]")]
[ApiController]
[Authorize]

public class DumbController : ControllerBase
{
    private readonly JwtManager _jwtmanager;
    public DumbController(JwtManager jwtManager)
    {
        _jwtmanager = jwtManager;
    }
    [HttpGet]
    public string Message()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var valid = _jwtmanager.VerifyTokenJwk(token);
        Console.WriteLine(valid);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var tokenS = jsonToken as JwtSecurityToken;
        var username = tokenS.Claims.First(x => x.Type == "sub");
        var password = tokenS.Claims.First(x => x.Type.ToString().Equals("password", StringComparison.InvariantCultureIgnoreCase));
        return "Hola mundo! " + username + " " + password;
    }
    [HttpGet]
    [Route("ValidRequest")]
    public async Task<IActionResult> ViewValidRequest()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var valid = _jwtmanager.VerifyTokenJwk(token);
        Console.WriteLine(valid);
        return Ok(_jwtmanager.Jwks());
    }
    [HttpGet]
    [Route("jwks")]
    public async Task<IActionResult> GetPublicKeys()
    {
        return Ok(_jwtmanager.Jwks());
    }
}