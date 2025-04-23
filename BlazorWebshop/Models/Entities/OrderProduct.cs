namespace BlazorWebshop.Models.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }

        // Foreign Keys 
        public int OrderId { get; set; }

        //public int ProductId { get; set; } // optioneel 

        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Navigation Property
        public Order Order { get; set; }
    }
}
