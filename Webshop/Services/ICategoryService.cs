using Webshop.Models.Entities;

namespace Webshop.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();

        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category, int id);

        Task DeleteCategoryAsync(int id);
    }
}
