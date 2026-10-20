
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ApiContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo; 
    }

    // POST - Opret bruger
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser(
        [FromBody] CreateUserDto request)
    {
        User user = new User
        {
            Username = request.Username,
            Password = request.Password
        };

        User created = await userRepo.AddAsync(user);

        UserDto dto = new UserDto
        {
            Id = created.Id,
            Username = created.Username
        };

        return Created($"/Users/{dto.Id}", dto);
    }

    // GET - Hent én bruger
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(
        [FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);

            UserDto dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };

            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // GET - Hent alle brugere eller filtrer
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers(
        [FromQuery] string? usernameContains)
    {
        var users = userRepo.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(usernameContains))
        {
            users = users.Where(u =>
                u.Username.ToLower().Contains(
                    usernameContains.ToLower()));
        }

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        }).ToList();

        return Ok(userDtos);
    }

    // PUT - Opdater bruger
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] int id,
        [FromBody] CreateUserDto request)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);

            user.Username = request.Username;
            user.Password = request.Password;

            await userRepo.UpdateAsync(user);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // DELETE - Slet bruger
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
