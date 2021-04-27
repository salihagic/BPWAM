using BPWA.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.DAL.Database
{
    public class DatabaseContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        #region Tables

        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<BusinessUnitUser> BusinessUnitUsers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryCurrency> CountryCurrencies { get; set; }
        public DbSet<CountryLanguage> CountryLanguages { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Translation> Translations { get; set; }

        #endregion

        #region Configure Identity entities

        void ConfigureRole(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Role>().HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
            builder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        }

        void ConfigureRoleClaim(ModelBuilder builder)
        {
            builder.Entity<RoleClaim>().ToTable("RoleClaims");
            builder.Entity<RoleClaim>().HasOne(x => x.Role).WithMany(x => x.RoleClaims).HasForeignKey(x => x.RoleId);
        }

        void ConfigureUser(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users");
            builder.Entity<User>().HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.Entity<User>().HasMany(x => x.UserClaims).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.Entity<User>().HasMany(x => x.UserLogins).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.Entity<User>().HasMany(x => x.UserTokens).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }

        void ConfigureUserClaim(ModelBuilder builder)
        {
            builder.Entity<UserClaim>().ToTable("UserClaims");
            builder.Entity<UserClaim>().HasOne(x => x.User).WithMany(x => x.UserClaims).HasForeignKey(x => x.UserId);
        }

        void ConfigureUserLogin(ModelBuilder builder)
        {
            builder.Entity<UserLogin>().ToTable("UserLogins");
            builder.Entity<UserLogin>().HasOne(x => x.User).WithMany(x => x.UserLogins).HasForeignKey(x => x.UserId);
        }

        void ConfigureUserRole(ModelBuilder builder)
        {
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<UserRole>().HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            builder.Entity<UserRole>().HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
        }

        void ConfigureUserToken(ModelBuilder builder)
        {
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserToken>().HasOne(x => x.User).WithMany(x => x.UserTokens).HasForeignKey(x => x.UserId);
        }

        #endregion 

        #region Helpers 

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SetIsDeletedFilters(builder);

            ConfigureRole(builder);
            ConfigureRoleClaim(builder);
            ConfigureUser(builder);
            ConfigureUserClaim(builder);
            ConfigureUserLogin(builder);
            ConfigureUserRole(builder);
            ConfigureUserToken(builder);
        }

        void SetIsDeletedFilters(ModelBuilder builder)
        {
            var entities = builder.Model.GetEntityTypes().Where(x => typeof(IBaseEntity).IsAssignableFrom(x.ClrType));

            foreach (var entity in entities)
            {
                var parameter = Expression.Parameter(entity.ClrType);

                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));

                #region IsDeleted

                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                BinaryExpression isDeletedCompareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
                var isDeletedLabmda = Expression.Lambda(isDeletedCompareExpression, parameter);
                builder.Entity(entity.ClrType).HasQueryFilter(isDeletedLabmda);

                #endregion
            }
        }

        #region SaveChanges overrides

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity);

            foreach (var entity in entities)
            {
                var iEntity = (IBaseEntity)entity.Entity;

                if (entity.State == EntityState.Added)
                    iEntity.CreatedAtUtc = DateTime.UtcNow;
                if (entity.State == EntityState.Modified)
                    iEntity.ModifiedAtUtc = DateTime.UtcNow;
                if (entity.State == EntityState.Deleted)
                {
                    entity.State = EntityState.Modified;
                    iEntity.IsDeleted = true;
                    iEntity.DeletedAtUtc = DateTime.UtcNow;
                    HandleCascadeSoftDelete(entity);
                }
            }
        }

        #endregion

        /// <summary>
        /// Works only on included related entities
        /// </summary>
        /// <param name="entity"></param>
        private void HandleCascadeSoftDelete(EntityEntry entity)
        {
            foreach (var navigationEntry in entity.Navigations.Where(n => !(n.Metadata as INavigation).IsOnDependent))
            {
                if (navigationEntry is CollectionEntry collectionEntry)
                {
                    if (collectionEntry.CurrentValue != null)
                    {
                        foreach (var dependentEntry in collectionEntry.CurrentValue)
                        {
                            if (dependentEntry != null)
                            {
                                var idependentEntry = (IBaseEntity)dependentEntry;
                                idependentEntry.IsDeleted = true;
                            }
                        }
                    }
                }
                else
                {
                    var dependentEntry = navigationEntry.CurrentValue;
                    if (dependentEntry != null)
                    {
                        var iNavigationEntry = (IBaseEntity)navigationEntry;
                        iNavigationEntry.IsDeleted = true;
                    }
                }
            }
        }

        #endregion
    }
}
