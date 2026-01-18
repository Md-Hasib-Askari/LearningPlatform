using LearningPlatform.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("verify")]
    public IActionResult VerifyUser()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var isValid = _authService.VerifyUserAsync(token);
        if (isValid)
        {
            return Ok(new BaseResponse { Success = true });
        }
        return Unauthorized(new BaseResponse { Success = false, ErrorMessage = "Invalid token." });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _authService.RegisterAsync(registerDto);
        return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        if (token != null)
        {
            return Ok(new LoginResponse { Success = true, Token = token });
        }
        return Unauthorized(new LoginResponse { Success = false, ErrorMessage = "Invalid email or password." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        await _authService.SendForgotPasswordTokenAsync(forgotPasswordDto);
        return Ok(new BaseResponse { Success = true });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        await _authService.ResetPasswordAsync(resetPasswordDto);
        return Ok(new BaseResponse { Success = true });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto changeUserRoleDto)
    {
        await _authService.ChangeUserRoleAsync(changeUserRoleDto);
        return Ok(new BaseResponse { Success = true });
    }
}