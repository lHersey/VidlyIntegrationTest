using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CleanVidly.Controllers.Resources;
using CleanVidly.Core.Abstract;
using CleanVidly.Core;
using CleanVidly.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CleanVidly.Controllers.Categories
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoriesController(IMapper mapper, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IEnumerable<KeyValuePairResource>> GetAllCategories()
        {
            var categories = await categoryRepository.GetFindAllAsync();
            return mapper.Map<IEnumerable<KeyValuePairResource>>(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var categpry = await categoryRepository.FindUniqueAsync(c => c.Id == categoryId);

            if (categpry is null) return NotFound("Category not found");

            return Ok(mapper.Map<KeyValuePairResource>(categpry));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddNewCategory(SaveCategoryResource saveCategoryResource)
        {
            var category = mapper.Map<Category>(saveCategoryResource);

            await categoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();

            var categoryResource = mapper.Map<KeyValuePairResource>(category);
            return Ok(categoryResource);
        }

        [Authorize]
        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateExistintCategory(int categoryId, SaveCategoryResource saveCategoryResource)
        {
            var category = await categoryRepository.FindUniqueAsync(c => c.Id == categoryId);
            if (category is null) return NotFound("Category not found");

            mapper.Map<SaveCategoryResource, Core.Entities.Category>(saveCategoryResource, category);

            await unitOfWork.SaveAsync();

            var categoryResource = mapper.Map<KeyValuePairResource>(category);
            return Ok(categoryResource);
        }

        [Authorize]
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteExistingCategory(int categoryId)
        {
            var category = await categoryRepository.FindUniqueAsync(c => c.Id == categoryId);
            if (category is null) return NotFound("Category not found");

            categoryRepository.Remove(category);
            await unitOfWork.SaveAsync();

            var categoryResource = mapper.Map<KeyValuePairResource>(category);
            return Ok(categoryResource);
        }
    }
}