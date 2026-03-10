using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIdAsync(Guid id);
        public void AddAsync(T entity);
        public void UpdateAsync(T entity);
        public void DeleteAsync(T entity);


    }
}
