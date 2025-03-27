using BlazorWebshop.Data;
using BlazorWebshop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class ProductService : IProductService
    {
        //private readonly ApplicationDbContext _context;

        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        //public ProductService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public ProductService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var result = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
            return result;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var product = await _context.Products
             .Include(p => p.Category)
             .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            await using var _context = _contextFactory.CreateDbContext();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product, int id)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct != null)
            {
                dbProduct.Name = product.Name;
                dbProduct.Description = product.Description;
                dbProduct.Price = product.Price;
                dbProduct.CategoryId = product.CategoryId;
                dbProduct.Stock = product.Stock;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProductExists(string productName, int? productId = null)
        {
            await using var _context = _contextFactory.CreateDbContext();
            return await _context.Products.AnyAsync(p => p.Name == productName && (productId == null || p.Id != productId));
        }

        public async Task<bool> DecreaseStockAsync(int productId)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.Stock <= 0)
            {
                return false; // Voor de zekerheid
            }

            product.Stock--;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncreaseStockAsync(int productId)
        {
            await using var _context = _contextFactory.CreateDbContext();
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return false; // Voor de zekerheid
            }

            product.Stock++;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}