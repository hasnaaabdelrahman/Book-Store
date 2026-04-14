namespace BookStore.Dtos.Incoming
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
