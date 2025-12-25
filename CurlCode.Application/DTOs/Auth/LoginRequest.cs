using System.ComponentModel.DataAnnotations;

namespace CurlCode.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Email or UserName is required")]
    [StringLength(256, ErrorMessage = "EmailOrUserName cannot exceed 256 characters")]
    [MinLength(1, ErrorMessage = "EmailOrUserName cannot be empty")]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(1, ErrorMessage = "Password cannot be empty")]
    public string Password { get; set; } = string.Empty;
}





