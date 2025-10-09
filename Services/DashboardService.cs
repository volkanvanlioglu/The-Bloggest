using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            try
            {
                // Get total counts
                var totalPosts = await _context.Posts.CountAsync();
                var totalComments = await _context.Comments.CountAsync();
                var totalUsers = await _context.Users.CountAsync();

                // Calculate trend percentages (comparing with previous month)
                var now = DateTime.UtcNow;
                var lastMonth = now.AddMonths(-1);
                var twoMonthsAgo = now.AddMonths(-2);

                var postsLastMonth = await _context.Posts
                    .Where(p => p.CreatedAt >= lastMonth && p.CreatedAt < now)
                    .CountAsync();

                var postsTwoMonthsAgo = await _context.Posts
                    .Where(p => p.CreatedAt >= twoMonthsAgo && p.CreatedAt < lastMonth)
                    .CountAsync();

                var commentsLastMonth = await _context.Comments
                    .Where(c => c.CreatedAt >= lastMonth && c.CreatedAt < now)
                    .CountAsync();

                var commentsTwoMonthsAgo = await _context.Comments
                    .Where(c => c.CreatedAt >= twoMonthsAgo && c.CreatedAt < lastMonth)
                    .CountAsync();

                // Note: Identity Users don't have CreatedAt by default, so we'll use a simplified approach
                // You could add a CreatedAt property to ApplicationUser if you need this functionality
                var usersLastMonth = 0; // Simplified for now
                var usersTwoMonthsAgo = 0; // Simplified for now

                // Calculate trend percentages
                var postsTrendPercentage = CalculateTrendPercentage(postsLastMonth, postsTwoMonthsAgo);
                var commentsTrendPercentage = CalculateTrendPercentage(commentsLastMonth, commentsTwoMonthsAgo);
                var usersTrendPercentage = CalculateTrendPercentage(usersLastMonth, usersTwoMonthsAgo);

                // Get monthly activity data for the last 12 months
                var monthlyPosts = await GetMonthlyActivityData("Posts");
                var monthlyComments = await GetMonthlyActivityData("Comments");

                // Get user role distribution
                var userRoleDistribution = await GetUserRoleDistribution();

                // Get top active users
                var topActiveUsers = await GetTopActiveUsers();

                return new DashboardData
                {
                    TotalPosts = totalPosts,
                    TotalComments = totalComments,
                    TotalUsers = totalUsers,
                    PostsTrendPercentage = postsTrendPercentage,
                    CommentsTrendPercentage = commentsTrendPercentage,
                    UsersTrendPercentage = usersTrendPercentage,
                    MonthlyPosts = monthlyPosts,
                    MonthlyComments = monthlyComments,
                    UserRoleDistribution = userRoleDistribution,
                    TopActiveUsers = topActiveUsers
                };
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a proper logging framework)
                Console.WriteLine($"Error fetching dashboard data: {ex.Message}");
                
                // Return fallback data
                return GetFallbackData();
            }
        }

        private static int CalculateTrendPercentage(int current, int previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0;
            return (int)Math.Round(((double)(current - previous) / previous) * 100);
        }

        private async Task<List<MonthlyActivity>> GetMonthlyActivityData(string type)
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                               "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            var result = new List<MonthlyActivity>();

            // Get data for the last 12 months
            var now = DateTime.UtcNow;
            
            for (int i = 11; i >= 0; i--)
            {
                var monthDate = now.AddMonths(-i).Date;
                var startOfMonth = new DateTime(monthDate.Year, monthDate.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);
                
                int count = 0;
                if (type == "Posts")
                {
                    count = await _context.Posts
                        .Where(p => p.CreatedAt >= startOfMonth && p.CreatedAt < endOfMonth)
                        .CountAsync();
                }
                else if (type == "Comments")
                {
                    count = await _context.Comments
                        .Where(c => c.CreatedAt >= startOfMonth && c.CreatedAt < endOfMonth)
                        .CountAsync();
                }

                result.Add(new MonthlyActivity
                {
                    Month = months[startOfMonth.Month - 1],
                    Count = count
                });
            }

            return result;
        }

        private async Task<Dictionary<string, int>> GetUserRoleDistribution()
        {
            try
            {
                // Get all users with their roles
                var usersWithRoles = await _context.UserRoles
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, RoleName = r.Name })
                    .Join(_context.Users, ur => ur.UserId, u => u.Id, (ur, u) => new { ur.RoleName, UserId = u.Id })
                    .GroupBy(x => x.RoleName)
                    .Select(g => new { Role = g.Key, Count = g.Count() })
                    .ToListAsync();

                var distribution = new Dictionary<string, int>();
                
                foreach (var role in usersWithRoles)
                {
                    distribution[role.Role ?? "Reader"] = role.Count;
                }

                // Add users without roles as "Reader"
                var usersWithRolesCount = distribution.Values.Sum();
                var totalUsers = await _context.Users.CountAsync();
                var readersCount = totalUsers - usersWithRolesCount;
                
                if (readersCount > 0)
                {
                    distribution["Reader"] = readersCount;
                }

                return distribution;
            }
            catch
            {
                // Fallback if there's an issue with roles
                return new Dictionary<string, int>
                {
                    { "Admin", 1 },
                    { "Author", 2 },
                    { "Reader", await _context.Users.CountAsync() - 3 }
                };
            }
        }

        private async Task<List<UserActivity>> GetTopActiveUsers()
        {
            var topUsers = await _context.Users
                .Select(u => new UserActivity
                {
                    Username = u.DisplayName ?? "Unknown",
                    PostCount = u.Posts.Count(),
                    CommentCount = u.Comments.Count(),
                    Role = "Reader" // Default role
                })
                .OrderByDescending(u => u.PostCount + u.CommentCount)
                .Take(5)
                .ToListAsync();

            // Update roles for users who have roles assigned
            foreach (var user in topUsers)
            {
                var userEntity = await _context.Users
                    .FirstOrDefaultAsync(u => (u.DisplayName ?? u.Email) == user.Username);
                
                if (userEntity != null)
                {
                    var userRole = await _context.UserRoles
                        .Where(ur => ur.UserId == userEntity.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .FirstOrDefaultAsync();
                    
                    if (!string.IsNullOrEmpty(userRole))
                    {
                        user.Role = userRole;
                    }
                }
            }

            return topUsers;
        }

        private DashboardData GetFallbackData()
        {
            return new DashboardData
            {
                TotalPosts = 0,
                TotalComments = 0,
                TotalUsers = 0,
                PostsTrendPercentage = 0,
                CommentsTrendPercentage = 0,
                UsersTrendPercentage = 0,
                MonthlyPosts = GetDefaultMonthlyData(),
                MonthlyComments = GetDefaultMonthlyData(),
                UserRoleDistribution = new Dictionary<string, int>
                {
                    { "Admin", 1 },
                    { "Author", 0 },
                    { "Reader", 0 }
                },
                TopActiveUsers = new List<UserActivity>()
            };
        }

        private List<MonthlyActivity> GetDefaultMonthlyData()
        {
            var months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            return months.Select(month => new MonthlyActivity
            {
                Month = month,
                Count = 0
            }).ToList();
        }
    }
}