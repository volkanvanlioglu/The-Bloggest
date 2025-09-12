namespace TheBloggest.Data.Models
{
    public class AuditLogs
    {
        public int Id { get; set; }
        public string EntityName { get; set; }   // e.g. "Post"
        public string EntityId { get; set; }     // e.g. "42"
        public string Action { get; set; }       // Create, Update, Delete
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Changes { get; set; } // Optional: JSON with before/after values
    }
}