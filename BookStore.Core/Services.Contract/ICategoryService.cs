using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Services.Contract
{
    public interface ICategoryService
    {
        public Task<IReadOnlyList<Category>> GetAllCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(Guid id);
        public Task<Category> CreateCategoryAsync(Category category);



    }
}
