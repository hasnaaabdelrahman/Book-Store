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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
              _unitOfWork.Repository<Category>().AddAsync(category);
              await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Repository<Category>().GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
           return await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        }
    }
}
