namespace TheBloggest.Data.Models
{
    public class Reaction
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ReactionType { get; set; } // e.g. "like", "love", "laugh", "angry"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}