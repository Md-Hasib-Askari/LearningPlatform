using Microsoft.AspNetCore.Mvc;
using TaskManagement.Business.Interfaces;

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("verify")]
    public IActionResult VerifyUser()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var isValid = _authService.VerifyUserAsync(token);
        if (isValid)
        {
            return Ok(new { Message = "User verified successfully." });
        }
        return Unauthorized(new { Message = "Invalid token." });
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
            return Ok(new { Token = token });
        }
        return Unauthorized(new { Message = "Invalid credentials." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        await _authService.SendForgotPasswordTokenAsync(forgotPasswordDto);
        return Ok(new { Message = "Password reset link sent to your email." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        await _authService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto);
        return Ok(new { Message = "Password has been reset successfully." });
    }
}