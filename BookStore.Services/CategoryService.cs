using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CategoryService : ICategoryService
    {
        public CategoryService() { }
        public Task<Category> CreateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
