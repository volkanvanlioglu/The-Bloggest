namespace TheBloggest.Data.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Key { get; set; }   // e.g. "SiteTitle"
        public string Value { get; set; } // e.g. "The Bloggest"
    }
}