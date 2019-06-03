namespace Idone.Security
{
    using Idone.Security.Services;

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
        public static IServiceCollection AddSecurityDi(this IServiceCollection services)
        {
            return services.AddScoped<UserService>();
        }
    }
}