using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace TheBloggest.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Post Post { get; set; }

        public string UserId { get; set; }   // Nullable for guest comments
        [JsonIgnore]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        public string AuthorName { get; set; } // for guests
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = true;
    }
}