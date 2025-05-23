using System.ComponentModel.DataAnnotations;

namespace AzureBlobManager.WebApi.Authentication.Models;

public class RegisterDto
{
    [Required, MinLength(5)]
    public string Username { get; init; }
    
    [Required]
    public string Email { get; init; }

    [Required, MinLength(5)]
    public string Password { get; init; }
}