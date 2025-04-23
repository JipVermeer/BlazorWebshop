namespace BlazorWebshop.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation Property
        public List<OrderProduct> OrderProducts { get; set; }



    }
}
