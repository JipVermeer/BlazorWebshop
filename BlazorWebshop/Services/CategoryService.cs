using BlazorWebshop.Data;
using BlazorWebshop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public CategoryService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            await using var _context = _contextFactory.CreateDbContext();
            var result = await _context.Categories.ToListAsync();
            return result;
        }

        public async Task<List<Category>> GetAllCategoriesWithProductsAsync()
        {
            await using var _context = _contextFactory.CreateDbContext();
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            await using var _context = _contextFactory.CreateDbContext();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var category = await _context.Categories.FindAsync(id);
            // not having a catg is not handled yet 
            return category;
        }

        public async Task UpdateCategoryAsync(Category category, int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var dbCategory = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                dbCategory.Name = category.Name;
                dbCategory.Description = category.Description;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CategoryExists(string categoryName, int? categoryId = null)
        {
            await using var _context = _contextFactory.CreateDbContext();
            return await _context.Categories.AnyAsync(c => c.Name == categoryName && (categoryId == null || c.Id != categoryId));
        }
    }
}
