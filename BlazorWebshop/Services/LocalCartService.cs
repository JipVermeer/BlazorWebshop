namespace BlazorWebshop.Services
{
    using Blazored.LocalStorage;
    using BlazorWebshop.Models.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LocalCartService
    {
        private readonly ILocalStorageService _localStorage;

        public event Action? OnChange;

        public LocalCartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<int> GetCartCountAsync()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            return cart?.Count ?? 0;
        }

        public async Task<decimal> GetTotalCartPrice()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null || cart.Count == 0)
            {
                return 0;
            }

            decimal totalPrice = 0;

            foreach (var item in cart)
            {
                totalPrice += item.Quantity * item.Product.Price;
            }

            return totalPrice;
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
        }

        // 'UpsertCartItem' zou misschien betere term zijn -> upsert = add new record of update existing one
        public async Task AddToCart(CartItem item)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            // Als product al bestaat, verander alleen quantity
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

        public async Task UpdateCartItemQuantity(CartItem item, int quantity)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            var itemInCart = cart.FirstOrDefault(i => i.Id == item.Id);

            if (itemInCart != null)
            {
                itemInCart.Quantity = quantity;
                await _localStorage.SetItemAsync("cart", cart);
                NotifyStateChanged();
            }

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

        public async Task EmptyCart()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            cart.Clear();
            //foreach (CartItem item in cart)
            //{
            //    cart.Remove(item);
            //}
            // Blijkbaar is bovenstaande niet handig want je itereert en verwijdert tegelijk
            await _localStorage.SetItemAsync("cart", cart);
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
