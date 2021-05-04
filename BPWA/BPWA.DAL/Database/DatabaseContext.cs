using BPWA.Core.Entities;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
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

        #region Configuration

        void ConfigureBusinessUnit(ModelBuilder builder)
        {
            builder.Entity<BusinessUnit>()
                .HasQueryFilter(x =>
                (!_currentCompany.Id().HasValue || x.CompanyId == _currentCompany.Id())
                && !x.IsDeleted);
        }

        void ConfigureBusinessUnitUser(ModelBuilder builder) { }

        void ConfigureCity(ModelBuilder builder) { }

        void ConfigureCompany(ModelBuilder builder) { }

        void ConfigureCompanyUser(ModelBuilder builder) { }

        void ConfigureCountry(ModelBuilder builder) { }

        void ConfigureCountryCurrency(ModelBuilder builder) { }

        void ConfigureCountryLanguage(ModelBuilder builder) { }

        void ConfigureCurrency(ModelBuilder builder) { }

        void ConfigureGroup(ModelBuilder builder) 
        {
            builder.Entity<Group>()
                .HasQueryFilter(x =>
                //Administration
                (!_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue) ||
                //Company
                (_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue && x.CompanyId == _currentCompany.Id()) ||
                //Business unit
                (_currentCompany.Id().HasValue && _currentBusinessUnit.Id().HasValue && x.CompanyId == _currentCompany.Id() && x.BusinessUnitId == _currentBusinessUnit.Id())
                && !x.IsDeleted);
        }

        void ConfigureGroupUser(ModelBuilder builder) { }

        void ConfigureLanguage(ModelBuilder builder) { }

        void ConfigureLog(ModelBuilder builder) { }

        void ConfigureNotification(ModelBuilder builder) { }

        void ConfigureNotificationGroup(ModelBuilder builder) { }

        void ConfigureNotificationLog(ModelBuilder builder) { }

        void ConfigureTicket(ModelBuilder builder) { }

        void ConfigureTranslation(ModelBuilder builder) { }

        #region Identity

        void ConfigureRole(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Role>().HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
            builder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);

            //Removing unique index Name from Roles table and creating new index Name+CompanyId+BusinessUnitId
            builder.Entity<Role>(builder =>
            {
                builder.Metadata.RemoveIndex(new[] { builder.Property(r => r.NormalizedName).Metadata });
                builder.HasIndex(x => new { x.NormalizedName, x.CompanyId, x.BusinessUnitId }).HasName("RoleNameIndex").IsUnique();
            });

            builder.Entity<Role>()
                .HasQueryFilter(x =>
                //Administration
                (!_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue)
                //Company
                || (_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue && x.CompanyId == _currentCompany.Id() && x.BusinessUnitId == null)
                //Business unit
                || (_currentCompany.Id().HasValue && _currentBusinessUnit.Id().HasValue && x.CompanyId == null && x.BusinessUnitId == _currentBusinessUnit.Id())
                && !x.IsDeleted);
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

            //EF does not allow this for now (https://docs.microsoft.com/en-us/ef/core/querying/filters)
            //builder.Entity<User>()
            //    .HasQueryFilter(x =>
            //    //Administration
            //    (!_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue) ||
            //    //Company
            //    (_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue && (x.CompanyUsers.Any(y => y.CompanyId == _currentCompany.Id()) || x.BusinessUnitUsers.Any(y => y.BusinessUnit.CompanyId == _currentCompany.Id()))) ||
            //    //Business unit
            //    (_currentCompany.Id().HasValue && _currentBusinessUnit.Id().HasValue && x.BusinessUnitUsers.Any(y => y.BusinessUnit.Id == _currentBusinessUnit.Id()))
            //    && !x.IsDeleted);
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

            //EF does not allow this for now (https://docs.microsoft.com/en-us/ef/core/querying/filters)
            //builder.Entity<UserRole>()
            //    .HasQueryFilter(x =>
            //    //Administration
            //    (!_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue) ||
            //    //Company
            //    (_currentCompany.Id().HasValue && !_currentBusinessUnit.Id().HasValue && x.Role.CompanyId == _currentCompany.Id() && x.Role.BusinessUnitId == null) ||
            //    //Business unit
            //    (_currentCompany.Id().HasValue && _currentBusinessUnit.Id().HasValue && x.Role.CompanyId == null && x.Role.BusinessUnitId == _currentBusinessUnit.Id())
            //    && !x.IsDeleted);
        }

        void ConfigureUserToken(ModelBuilder builder)
        {
            builder.Entity<UserToken>().ToTable("UserTokens");
            builder.Entity<UserToken>().HasOne(x => x.User).WithMany(x => x.UserTokens).HasForeignKey(x => x.UserId);
        }

        #endregion Identity

        #endregion Configuration

        #region Helpers 

        private ICurrentCompany _currentCompany;
        private ICurrentBusinessUnit _currentBusinessUnit;

        public DatabaseContext(
            DbContextOptions<DatabaseContext> options,
            ICurrentCompany currentCompany,
            ICurrentBusinessUnit currentBusinessUnit
            ) : base(options)
        {
            _currentCompany = currentCompany;
            _currentBusinessUnit = currentBusinessUnit;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SetIsDeletedFilters(builder);

            #region Configure specific models

            ConfigureBusinessUnit(builder);
            ConfigureBusinessUnitUser(builder);
            ConfigureCity(builder);
            ConfigureCompany(builder);
            ConfigureCompanyUser(builder);
            ConfigureCountry(builder);
            ConfigureCountryCurrency(builder);
            ConfigureCountryLanguage(builder);
            ConfigureCurrency(builder);
            ConfigureGroup(builder);
            ConfigureGroupUser(builder);
            ConfigureLanguage(builder);
            ConfigureLog(builder);
            ConfigureNotification(builder);
            ConfigureNotificationGroup(builder);
            ConfigureNotificationLog(builder);
            ConfigureTicket(builder);
            ConfigureTranslation(builder);

            #region Identity

            ConfigureRole(builder);
            ConfigureRoleClaim(builder);
            ConfigureUser(builder);
            ConfigureUserClaim(builder);
            ConfigureUserLogin(builder);
            ConfigureUserRole(builder);
            ConfigureUserToken(builder);

            #endregion

            #endregion
        }

        #region IsDeleted

        /// <summary>
        /// This method sets the x.IsDeleted == false filter to every query 
        /// on the entity that implements IBaseEntity interface
        /// Although this method sets the query filter, if you explicitly want
        /// to add new filters like CompanyId or BussinessUnitId, you will
        /// have to set IsDeleted filter by yourself for that entity
        /// see more: https://github.com/dotnet/efcore/issues/10275
        /// </summary>
        /// <param name="builder"></param>
        void SetIsDeletedFilters(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes().Where(x => typeof(IBaseEntity).IsAssignableFrom(x.ClrType)))
            {
                var parameter = Expression.Parameter(entity.ClrType);

                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));

                #region IsDeleted

                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
                BinaryExpression isDeletedCompareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
                var isDeletedLambda = Expression.Lambda(isDeletedCompareExpression, parameter);
                builder.Entity(entity.ClrType).HasQueryFilter(isDeletedLambda);

                #endregion
            }
        }

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

        #endregion IsDeleted

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
                else if (entity.State == EntityState.Modified)
                    iEntity.ModifiedAtUtc = DateTime.UtcNow;
                else if (entity.State == EntityState.Deleted &&
                    !HardDeleteTypes.Contains(entity.Entity.GetType()))
                {
                    entity.State = EntityState.Modified;
                    iEntity.IsDeleted = true;
                    iEntity.DeletedAtUtc = DateTime.UtcNow;
                    HandleCascadeSoftDelete(entity);
                }
            }
        }

        List<Type> HardDeleteTypes => new List<Type>
        {
            typeof(UserRole),
            typeof(Role),
        };

        #endregion

        #endregion
    }
}
