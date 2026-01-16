using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDto, cancellationToken);
        return Ok(updatedUser);
    }

    [HttpPut("me/password")]
    public async Task<IActionResult> UpdateCurrentUserPassword([FromBody] UpdatePasswordDto updatePasswordDto, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _userService.UpdateUserPasswordAsync(userId, updatePasswordDto, cancellationToken);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpGet("{id}/public-profile")]
    public async Task<IActionResult> GetUserPublicProfileById(Guid id, CancellationToken cancellationToken)
    {
        var publicProfile = await _userService.GetUserByIdAsync(id, cancellationToken);
        return Ok(publicProfile);
    }
}