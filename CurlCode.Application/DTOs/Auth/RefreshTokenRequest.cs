namespace CurlCode.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    public string EmailOrUserName { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}


