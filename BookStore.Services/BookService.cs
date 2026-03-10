using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class BookService : IBookService
    {

        public BookService()
        {
            
        }
        public Task<Book> CreateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> UpdateBookAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
