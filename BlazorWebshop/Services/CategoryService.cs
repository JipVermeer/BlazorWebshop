using BlazorWebshop.Models.Entities;
using BlazorWebshop.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var result = await _context.Categories.ToListAsync();
            return result;
        }

        public async Task<List<Category>> GetAllCategoriesWithProductsAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)  
                .ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            // not having a catg is not handled yet 
            return category;
        }

        public async Task UpdateCategoryAsync(Category category, int id)
        {
            var dbCategory = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                dbCategory.Name = category.Name;
                dbCategory.Description = category.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName);
        }
    }
}
