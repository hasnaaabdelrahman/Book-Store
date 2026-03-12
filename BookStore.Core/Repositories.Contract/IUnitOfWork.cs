using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories.Contract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<Book> Books { get; }        
        IGenericRepository<Category> Categories { get; }
        public Task<int> SaveAsync();

    }
}
