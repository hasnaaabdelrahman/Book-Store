using System.ComponentModel.DataAnnotations;

namespace BookStore.Dtos.outgoingDtos
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
