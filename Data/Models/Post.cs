using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheBloggest.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        public string Content { get; set; }

        public string Excerpt { get; set; }
        public string CoverImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public int ViewCount { get; set; } = 0;

        // Relationships
        public string AuthorId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ApplicationUser Author { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<PostCategory> PostCategories { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<PostTag> PostTags { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Comment> Comments { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Reaction> Reactions { get; set; }
    }
}