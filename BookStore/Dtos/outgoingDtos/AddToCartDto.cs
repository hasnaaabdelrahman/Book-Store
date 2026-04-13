using System.ComponentModel.DataAnnotations;

namespace BookStore.Dtos.outgoingDtos
{
    public class AddToCartDto
    {
        [Required]
        public Guid userId { get; set; }
        [Required]
        public Guid bookId { get; set; }
        [Required]
        public int quantity { get; set; }
    }
}
