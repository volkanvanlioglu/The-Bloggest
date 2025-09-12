namespace TheBloggest.Data.Models
{
    public class MediaLibrary
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }

        public string UploadedById { get; set; }
        public ApplicationUser UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}