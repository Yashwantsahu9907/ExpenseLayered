using System.ComponentModel.DataAnnotations;

namespace ExpenseLayeredApi.DTO;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
