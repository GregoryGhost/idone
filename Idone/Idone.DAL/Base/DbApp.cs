namespace Idone.DAL.Base
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Настройка БД приложения.
    /// </summary>
    public static class DbApp
    {
        /// <summary>
        /// Добавить настройки БД приложения <see cref="Idone"/>.
        /// </summary>
        /// <param name="serviceCollection"> Коллекция сервисов. </param>
        /// <param name="connection"> Строка подключения к БД. </param>
        /// <returns> Возвращает коллекцию сервисов. </returns>
        public static IServiceCollection AddIdoneDb(
            this IServiceCollection serviceCollection,
            string connection)
        {
            return serviceCollection
                .AddDbContext<AppContext>(options =>
                    options
                        //.UseLazyLoadingProxies()
                        .UseSqlServer(connection));
        }

        /// <summary>
        /// Добавить идентификацию пользователя приложения <see cref="Idone"/>.
        /// </summary>
        /// <param name="serviceCollection"> Коллекция сервисов. </param>
        /// <returns> Возвращает коллекцию сервисов. </returns>
        public static IServiceCollection AddIdoneIdentity(
            this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AppContext>()
                .AddDefaultTokenProviders();

            return serviceCollection;
        }
    }
}
