using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookServices;
        public BookController(IBookService bookService)
        {
            _bookServices = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Book>>> GetAllBooks()
        {
            var books = await _bookServices.GetAllBooksAsync();
            return Ok(books);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Book>> GetBookById(Guid id)
        {
            var book = await _bookServices.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
    }
}
