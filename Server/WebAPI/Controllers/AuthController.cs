using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    [HttpPost("login")]
    public ActionResult<UserDto> Login(
        [FromBody] LoginRequest request)
    {
        User? user = userRepo.GetManyAsync()
            .FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (user.Password != request.Password)
        {
            return Unauthorized();
        }

        UserDto dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(dto);
    }
}