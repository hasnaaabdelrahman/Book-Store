using BookStore.Core.Entities;
using BookStore.Core.Repositories.Contract;
using BookStore.Repository.Data;
using BookStore.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<Book> Books => Repository<Book>();

        public IGenericRepository<Category> Categories => Repository<Category>();

        private readonly ApplicationDbContext _context;

        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.TryGetValue(type, out var repo))
            {
                repo = new GenericRepository<T>(_context);
                _repositories[type] = repo;
            }

            return (IGenericRepository<T>)repo;
        }

        public async ValueTask DisposeAsync()
        {
           await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
