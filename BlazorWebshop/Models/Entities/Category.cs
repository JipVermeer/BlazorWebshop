﻿using System.Text.Json.Serialization;

namespace BlazorWebshop.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
