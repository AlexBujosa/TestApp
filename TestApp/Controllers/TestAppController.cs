namespace TestApp.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestAppController : ControllerBase
{
    [HttpGet]
    public async Task<string> MessageAsync()
    {
        Task<string> message = Task.FromResult("");
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        HttpClientInt http = new HttpClientInt();
        if (token != "Bearer")
        {
            message = http.GetPublicKeys(token);
        }
        if (await message == "Token is valid")
        {
            return "Hello World!";
        }
        return "No Hello World! para ti";
    }
}