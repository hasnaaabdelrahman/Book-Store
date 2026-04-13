namespace BookStore.Dtos.outgoingDtos
{
    public class RemoveFromCartDto
    {
        public Guid userId { get; set; }
        public Guid bookId { get; set; }
    }
}
