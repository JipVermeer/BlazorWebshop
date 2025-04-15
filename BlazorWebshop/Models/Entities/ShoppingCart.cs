using BlazorWebshop.Data;

namespace BlazorWebshop.Models.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign Key (Identity gebruikt string voor IDs)

        // Navigation properties

        // [JsonIgnore]
        public ApplicationUser User { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
    }
}
