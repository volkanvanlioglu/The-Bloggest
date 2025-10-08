namespace TheBloggest.Data.Models
{
    public class DashboardData
    {
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalUsers { get; set; }
        public int PostsTrendPercentage { get; set; }
        public int CommentsTrendPercentage { get; set; }
        public int UsersTrendPercentage { get; set; }
        public List<MonthlyActivity> MonthlyPosts { get; set; } = new();
        public List<MonthlyActivity> MonthlyComments { get; set; } = new();
        public Dictionary<string, int> UserRoleDistribution { get; set; } = new();
        public List<UserActivity> TopActiveUsers { get; set; } = new();
    }

    public class MonthlyActivity
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class UserActivity
    {
        public string Username { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
