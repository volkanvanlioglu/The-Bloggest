using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace TheBloggest.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<PostTag> PostTags { get; set; }
    }
}