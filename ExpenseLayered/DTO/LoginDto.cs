using System.ComponentModel.DataAnnotations;

namespace ExpenseLayeredApi.DTO
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
