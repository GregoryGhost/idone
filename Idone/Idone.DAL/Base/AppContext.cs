namespace Idone.DAL.Base
{
    using System;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.Entities;

    using LanguageExt;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using static LanguageExt.Prelude;

    /// <summary>
    /// Контекст приложения.
    /// </summary>
    public class AppContext : IdentityDbContext<User, Role, int>
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="options"> Опции контекста. </param>
        public AppContext(DbContextOptions options)
            : base(options)
        {
        }

        ///// <summary>
        ///// Права доступа.
        ///// </summary>
        //public DbSet<Permission> Persmissions { get; set; }

        ///// <summary>
        ///// Права доступа для ролей.
        ///// </summary>
        //public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        ///// <summary>
        ///// Роли пользователей.
        ///// </summary>
        //public DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Инициализировать БД.
        /// </summary>
        public void Init()
        {
            Database.Migrate();
        }

        /// <summary>
        /// Тестовая инициализация БД.
        /// </summary>
        public void InitTest()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        /// <summary>
        /// Попытаться сохранить изменения в БД.
        /// </summary>
        /// <returns> Результат. </returns>
        public Either<Error, bool> TrySaveChanges()
        {
            try
            {
                SaveChanges();
                return Right<Error, bool>(true);
            }
            catch (Exception e)
            {
                //TODO: писать в лог полное сообщение об ошибке.
                return Left(Error.Exception);
            }
        }

        /// <summary>
        /// Установить конфигурацию для сущностей БД при построении.
        /// </summary>
        /// <param name="modelBuilder"> Построитель моделей БД. </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}