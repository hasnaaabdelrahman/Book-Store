namespace BookStore.Dtos.Incoming
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

    }
}
