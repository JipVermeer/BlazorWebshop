using BlazorWebshop.Models.Entities;

namespace BlazorWebshop.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task UpdateProductAsync(Product product, int id);
        Task<bool> ProductExists(string productName, int? id);
    }
}
