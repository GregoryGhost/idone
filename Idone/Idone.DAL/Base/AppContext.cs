namespace Idone.DAL.Base
{
    using System;

    using Idone.DAL.Dictionaries;
    using Idone.DAL.Entities;

    using LanguageExt;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        /// <summary>
        /// Права доступа.
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Права доступа для ролей.
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public new DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Роли пользователей.
        /// </summary>
        public new DbSet<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public new DbSet<User> Users { get; set; }

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
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserRoleConfig());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(ur => ur.User).WithMany(user => user.UserRoles).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ur => ur.Role).WithMany(role => role.UserRoles).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("RoleId", "UserId").IsUnique();
        }
    }

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.OwnsOne(user => user.FullName);
        }
    }
}