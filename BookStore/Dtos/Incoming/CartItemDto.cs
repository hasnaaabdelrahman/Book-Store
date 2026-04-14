namespace BookStore.Dtos.Incoming
{
    public class CartItemDto
    {

        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
