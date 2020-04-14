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
        /// <returns> Возвращает сервисы. </returns>
        public static IServiceCollection AddSecurityDi(this IServiceCollection services, IConfiguration config)
        {
            //TODO: при разрастании параметров выделить в отдельный класс десериализации настроек
            var adDomain = config.GetSection("ActiveDirectory").GetSection("domain").Value;

            services.AddScoped<UserService>();
            services.AddScoped(s => new AdService(adDomain));

            return services;
        }
    }
}