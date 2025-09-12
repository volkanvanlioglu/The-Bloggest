namespace TheBloggest.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
    }
}