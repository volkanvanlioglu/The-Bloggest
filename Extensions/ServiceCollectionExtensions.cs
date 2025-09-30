using Microsoft.Extensions.Options;
using TheBloggest.Configuration;
using TheBloggest.Interfaces;
using TheBloggest.Services;

namespace TheBloggest.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlogServices(this IServiceCollection services)
        {
            // Bind ApiSettings (from appsettings.json)
            services.AddOptions<ApiSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection("ApiSettings").Bind(settings);
                    });

            // Reuse generic method to register each service
            services.AddApiClient<IPostService, PostService>();
            services.AddApiClient<ICategoryService, CategoryService>();
            services.AddApiClient<ITagService, TagService>();
            services.AddApiClient<ICommentService, CommentService>();
            services.AddApiClient<IReactionService, ReactionService>();
            services.AddApiClient<IMediaLibraryService, MediaLibraryService>();
            services.AddApiClient<ISettingsService, SettingsService>();
            services.AddApiClient<IAuditLogsService, AuditLogsService>();

            return services;
        }

        // Generic helper for all HttpClient services
        private static IServiceCollection AddApiClient<TInterface, TImplementation>(
            this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddHttpClient<TInterface, TImplementation>((sp, client) =>
            {
                var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            return services;
        }
    }
}