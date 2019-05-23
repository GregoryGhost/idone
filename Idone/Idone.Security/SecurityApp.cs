namespace Idone.Security
{
    using Idone.Security.Services;

    using Microsoft.Extensions.DependencyInjection;

    public static class SecurityApp
    {
        public static IServiceCollection AddSecurityDi(this IServiceCollection services)
        {
            return services.AddScoped<UserService>();
        }
    }
}