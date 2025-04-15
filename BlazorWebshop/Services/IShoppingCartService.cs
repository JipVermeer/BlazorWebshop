using BlazorWebshop.Models.Entities;

namespace BlazorWebshop.Services
{
    public interface IShoppingCartService
    {
        event Action? OnChange;
        Task<ShoppingCart> GetUserShoppingCartAsync(string userId);
        Task AddCartItemToUserCart(string userId, CartItem item);
        Task RemoveCartItemFromUserCart(string userId, CartItem item);
        Task UpdateUserCartItemQuantity(string userId, CartItem item, int quantity);
        Task<int> GetShoppingCartCountAsync(string userId);
        Task<decimal> GetTotalCartPrice(string userId);
        Task MergeCarts(List<CartItem> guestCartItems, string userId);

        void NotifyStateChanged();
    }
}
