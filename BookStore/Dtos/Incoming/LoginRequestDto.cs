using System.ComponentModel.DataAnnotations;

namespace BookStore.Dtos.Incoming
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
