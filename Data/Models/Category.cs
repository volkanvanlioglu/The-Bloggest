﻿namespace TheBloggest.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public ICollection<PostCategory> PostCategories { get; set; }
    }
}