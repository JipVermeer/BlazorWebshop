﻿namespace Webshop.Models.Entities
{
    public class VideoGame
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Publisher { get; set; }
        public int? ReleaseYear { get; set; }
    }
}
