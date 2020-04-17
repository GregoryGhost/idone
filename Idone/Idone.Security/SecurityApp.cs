namespace Idone.Security
{
    using Idone.Security.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Настройки модуля "Администрирование".
    /// </summary>
    public static class SecurityApp
    {
        /// <summary>
        /// Добавить зависимости модуля.
        /// </summary>
        /// <param name="services"> Сервисы. </param>
        /// <param name="adDomain"> Домен Active Directory сервиса. </param>
        /// <returns> Возвращает сервисы. </returns>
        public static IServiceCollection AddSecurityDi(this IServiceCollection services, string adDomain)
        {
            services.AddScoped<UserService>();
            services.AddScoped(s => new AdService(adDomain));

            return services;
        }
    }
}