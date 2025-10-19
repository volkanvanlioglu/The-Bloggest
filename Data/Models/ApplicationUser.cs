using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace TheBloggest.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        // Navigation

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Post> Posts { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Comment> Comments { get; set; }
    }
}