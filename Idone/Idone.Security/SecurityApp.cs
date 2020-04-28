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
        /// <param name="adDomain"> Домен Active Directory сервиса. </param>
        /// <param name="adLogin">Логин в LDAP аля "cn=admin,dc=example,dc=org".</param>
        /// <returns> Возвращает сервисы. </returns>
        public static IServiceCollection AddSecurityDi(this IServiceCollection services, string adDomain, string adLogin,
            string adPswd)
        {
            services.AddScoped<UserService>();
            services.AddScoped(s => new AdService(adDomain, adLogin, adPswd));

            return services;
        }
    }
}