using BlazorWebshop.Models.Entities;

namespace BlazorWebshop.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task PlaceOrderAsync(ShoppingCart ShoppingCart);
    }
}
