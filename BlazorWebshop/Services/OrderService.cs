using BlazorWebshop.Data;
using BlazorWebshop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebshop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public OrderService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            await using var _context = _contextFactory.CreateDbContext();
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderProducts)
                .ToListAsync();
        }

        public async Task PlaceOrderAsync(ShoppingCart ShoppingCart)
        {
            await using var _context = _contextFactory.CreateDbContext();

            Order order = new Order
            {
                UserId = ShoppingCart.UserId,
                OrderDate = DateTime.Now, // Eigenlijk is UtcNow beter 
                OrderProducts = new List<OrderProduct>()
            };

            foreach (CartItem item in ShoppingCart.CartItems)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductName = item.Product.Name,
                    ProductDescription = item.Product.Description,
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                });
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
