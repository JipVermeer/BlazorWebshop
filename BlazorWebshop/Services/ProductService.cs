using BlazorWebshop.Models.Entities;
using BlazorWebshop.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var result = await _context.Products
                .Where(p => p.CategoryId == categoryId)  
                .ToListAsync();
            return result;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product, int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct != null)
            {
                dbProduct.Name = product.Name;
                dbProduct.Description = product.Description;
                dbProduct.Price = product.Price;
                dbProduct.CategoryId = product.CategoryId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProductExists(string productName)
        {
            return await _context.Products.AnyAsync(p => p.Name == productName);
        }
    }
}