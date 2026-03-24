using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using BookStore.Dtos.Incoming;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var createdCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = createCategoryDto.Name
            };
            await _categoryService.CreateCategoryAsync(createdCategory);
            return Ok(createdCategory);
        }
        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryService.GetCategoryByIdAsync(updateCategoryDto.Id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = updateCategoryDto.Name;
            await _categoryService.UpdateCategoryAsync(category);
            return Ok(category);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
