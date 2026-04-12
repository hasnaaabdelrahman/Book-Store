using System.ComponentModel.DataAnnotations;

namespace BookStore.Dtos.Incoming
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
