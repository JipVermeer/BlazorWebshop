using BlazorWebshop.Models.Entities;

namespace BlazorWebshop.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetAllCategoriesWithProductsAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category, int id);
        Task DeleteCategoryAsync(int id);
        Task<bool> CategoryExists(string categoryName, int? id);
    }
}
