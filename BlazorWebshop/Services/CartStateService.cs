namespace BlazorWebshop.Services
{
    using Blazored.LocalStorage;
    using BlazorWebshop.Models.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CartStateService
    {
        private readonly ILocalStorageService _localStorage;

        public event Action? OnChange;

        public CartStateService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<int> GetCartCountAsync()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            return cart?.Count ?? 0;
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
        }

        public async Task AddToCart(CartItem item)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            await _localStorage.SetItemAsync("cart", cart);
            NotifyStateChanged();
        }

        public async Task RemoveFromCartAsync(CartItem item)
        {
            // Als product bestaat, als er meer dan 1 zijn verander quantity, anders (als 1) haal hele item uit mandje

            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                if (existingItem.Quantity > 1)
                {
                    existingItem.Quantity -= 1;
                }
                else
                {
                    cart.Remove(existingItem);
                }
            }

            await _localStorage.SetItemAsync("cart", cart);
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
