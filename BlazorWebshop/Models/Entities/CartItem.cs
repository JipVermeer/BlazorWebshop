namespace BlazorWebshop.Models.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        // Foreign Keys
        public int ProductId { get; set; }
        public int? ShoppingCartId { get; set; }

        // Navigation properties
        // [JsonIgnore]
        public Product? Product { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
    }
}
