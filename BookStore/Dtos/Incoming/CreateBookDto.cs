namespace BookStore.Dtos.Incoming
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
