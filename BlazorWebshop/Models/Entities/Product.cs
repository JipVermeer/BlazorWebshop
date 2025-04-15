using System.Text.Json.Serialization;

namespace BlazorWebshop.Models.Entities
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Evt later toevoegen
        // public string ImgPath { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property

        [JsonIgnore] // Vanwege Js interop error 
        public Category Category { get; set; }
    }

}
