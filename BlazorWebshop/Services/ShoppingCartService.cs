using BlazorWebshop.Data;
using BlazorWebshop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public event Action? OnChange;

        public ShoppingCartService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<ShoppingCart> GetUserShoppingCartAsync(string userId)
        {
            // The await 'using' ensures that the DbContext is disposed of asynchronously as well when the scope ends (aldus internet lol)
            await using var _context = _contextFactory.CreateDbContext();

            var result = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            return result ?? new ShoppingCart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };
        }

        public async Task AddCartItemToUserCart(string userId, CartItem item)
        {
            // Nieuwe context is hier vereist, als je bovenstaande methode gebruikt is de context disposed tegen de tijd dat je deze weer wil gebruiken
            await using var _context = _contextFactory.CreateDbContext();

            ShoppingCart cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
            }

            CartItem existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                // _context.CartItems.Update(existingItem); // check ff of dit noodzakelijk is of savechangesasync al prima is
            }
            else
            {
                // Link nieuwe item aan cart 
                // item.ShoppingCartId = cart.Id; // Dit zou automatisch moeten gebeuren 
                cart.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
            NotifyStateChanged();
        }

        public async Task RemoveCartItemFromUserCart(string userId, CartItem item)
        {
            await using var _context = _contextFactory.CreateDbContext();

            ShoppingCart cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            CartItem existingItem = cart.CartItems.FirstOrDefault(c => c.ProductId == item.ProductId);


            if (existingItem != null)
            {
                //if (existingItem.Quantity > 1)
                //{
                //    existingItem.Quantity -= 1;
                //}
                //else
                //{
                _context.CartItems.Remove(existingItem);
                //}
            }

            await _context.SaveChangesAsync();
            NotifyStateChanged();
        }

        public async Task UpdateUserCartItemQuantity(string userId, CartItem item, int quantity)
        {
            await using var _context = _contextFactory.CreateDbContext();

            ShoppingCart cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            //if (cart == null || cart.CartItems == null)
            //    return; // Vraag me altijd af of dit nou wel of niet moet? Vraag ff

            CartItem itemInCart = cart.CartItems.FirstOrDefault(i => i.Id == item.Id);

            if (itemInCart != null)
            {
                itemInCart.Quantity = quantity;
                await _context.SaveChangesAsync();
                NotifyStateChanged();
            }
        }


        public async Task<int> GetShoppingCartCountAsync(string userId)
        {
            await using var _context = _contextFactory.CreateDbContext();

            ShoppingCart cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            int totalQuantity = 0;

            if (cart != null)
            {
                List<CartItem> cartItems = cart.CartItems;
                foreach (var item in cartItems)
                {
                    totalQuantity += item.Quantity;
                }
            }
            return totalQuantity;
        }
        public async Task<decimal> GetTotalCartPrice(string userId)
        {
            await using var _context = _contextFactory.CreateDbContext();

            ShoppingCart cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            List<CartItem> cartItems = cart.CartItems;

            if (cartItems == null || cartItems.Count == 0)
            {
                return 0;
            }

            decimal totalPrice = 0;

            foreach (CartItem item in cartItems)
            {
                totalPrice += item.Quantity * item.Product.Price;
            }
            return totalPrice;
        }

        public async Task MergeCarts(List<CartItem> guestCartItems, string userId)
        {
            await using var _context = _contextFactory.CreateDbContext();

            var cart = await _context.ShoppingCarts
                .Where(u => u.UserId == userId)
                .Include(sc => sc.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
            }

            foreach (var guestItem in guestCartItems)
            {
                // Reset Id om tracking conflict op te lossen
                guestItem.Id = 0;

                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == guestItem.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += guestItem.Quantity;

                    // Check of dit de voorraad niet overschrijdt
                    if (existingItem.Quantity > existingItem.Product.Stock)
                    {
                        existingItem.Quantity = existingItem.Product.Stock;
                    }
                }
                else
                {
                    //cart.CartItems.Add(guestItem);
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = guestItem.ProductId,
                        Quantity = guestItem.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            NotifyStateChanged();
        }

        public async Task ClearCart(string userId)
        {
            await using var _context = _contextFactory.CreateDbContext();

            var cart = await _context.ShoppingCarts
               .Where(u => u.UserId == userId)
               .Include(sc => sc.CartItems)
               .ThenInclude(ci => ci.Product)
               .FirstOrDefaultAsync();


            if (cart != null && cart.CartItems.Any())
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
                NotifyStateChanged();
            }
        }

        public void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
