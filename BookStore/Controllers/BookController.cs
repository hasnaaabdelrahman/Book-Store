using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using BookStore.Dtos.Incoming;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IReadOnlyList<Book>>> GetAllBooks()
        {
            var books = await _bookServices.GetAllBooksAsync();
            return Ok(books);
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Book>> GetBookById(Guid id)
        {
            var book = await _bookServices.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpGet]
        [Route("search/{title}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Book>> GetBookByTitle(string title)
        {
            var book = await _bookServices.GetBookByTitleAsync(title);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
    
            await _bookServices.DeleteBookAsync(id);
            return NoContent();

        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Book>> UpdateBook(UpdateBookDto updateBookDto)
        {
            var book = await _bookServices.GetBookByIdAsync(updateBookDto.Id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = updateBookDto.Title;
            book.Price = updateBookDto.Price;
            await _bookServices.UpdateBookAsync(book);
            return Ok(book);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Book>> CreateBook(CreateBookDto createBookDto)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = createBookDto.Title,
                Price = createBookDto.Price,
                CategoryId = createBookDto.CategoryId
            };
            await _bookServices.CreateBookAsync(book);
            return Ok(book);

        }
    }
}
