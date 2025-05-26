using System.ComponentModel.DataAnnotations;

namespace Fiap.CloudGames.Fase1.Application.DTOs;
public class RegisterUserDto
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = default!;
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "The password must contain at least one letter, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;
}
