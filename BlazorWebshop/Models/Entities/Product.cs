namespace BlazorWebshop.Models.Entities
{

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            //public string ImgPath { get; set; }

            public int CategoryId { get; set; } // Fk
            public Category Category { get; set; } // Nav prop
        }
    
}
