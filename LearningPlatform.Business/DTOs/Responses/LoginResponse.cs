using System.ComponentModel.DataAnnotations;

public sealed class LoginResponse : BaseResponse
{
    public string? Token { get; set; }
}