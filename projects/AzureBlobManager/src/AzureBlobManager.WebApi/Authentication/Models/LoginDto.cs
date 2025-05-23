using System.ComponentModel.DataAnnotations;

namespace AzureBlobManager.WebApi.Authentication.Models;

public class LoginDto
{
    [Required, MinLength(5)]
    public required string Username { get; init; }

    [Required, MinLength(5)]
    public required string Password { get; init; }
}