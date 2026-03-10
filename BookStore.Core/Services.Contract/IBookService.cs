using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Services.Contract
{
    public interface IBookService
    {
        Task<IReadOnlyList<Book>> GetAllBooksAsync();
        Task <Book> GetBookByIdAsync(Guid id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task DeleteBookAsync(Guid id);

    }
}
