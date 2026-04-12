using BookStore.Core.Entities;
using BookStore.Core.Repositories.Contract;
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
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            _unitOfWork.Repository<Book>().AddAsync(book);
            await _unitOfWork.SaveAsync();
            return book;
        }

        public async Task DeleteBookAsync(Guid id)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(id);
             _unitOfWork.Repository<Book>().DeleteAsync(book);
             await _unitOfWork.SaveAsync();
        }

        public async Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
           return await _unitOfWork.Repository<Book>().GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
           return await _unitOfWork.Repository<Book>().GetByIdAsync(id);

        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            return await _unitOfWork.Repository<Book>().GetByTitleAsync(title);
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
           _unitOfWork.Repository<Book>().UpdateAsync(book);
            await _unitOfWork.SaveAsync();
            return book;

        }
    }
}
