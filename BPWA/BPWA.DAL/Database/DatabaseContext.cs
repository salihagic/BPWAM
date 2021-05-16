using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.DAL.Database
{
    public class DatabaseContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        #region Tables

        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyActivityStatusLog> CompanyActivityStatusLogs { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
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

        #region Identity

        void ConfigureRole(ModelBuilder builder)
        {
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<Role>().HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
            builder.Entity<Role>().HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);

            //Removing unique index Name from Roles table and creating new index Name+CompanyId
            builder.Entity<Role>(builder =>
            {
                builder.Metadata.RemoveIndex(new[] { builder.Property(r => r.NormalizedName).Metadata });
                builder.HasIndex(x => new { x.NormalizedName, x.CompanyId }).HasName("RoleNameIndex").IsUnique();
            });
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

        #endregion Identity

        #region Helpers 

        private ICurrentCompany _currentCompany;
        private ICurrentBaseCompany _currentBaseCompany;

        public DatabaseContext(
            DbContextOptions<DatabaseContext> options,
            ICurrentCompany currentCompany,
            ICurrentBaseCompany currentBaseCompany
            ) : base(options)
        {
            _currentCompany = currentCompany;
            _currentBaseCompany = currentBaseCompany;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetGlobalFilters(builder);
            SetGlobalCascadeDelete(builder);

            #region Identity

            ConfigureRole(builder);
            ConfigureRoleClaim(builder);
            ConfigureUser(builder);
            ConfigureUserClaim(builder);
            ConfigureUserLogin(builder);
            ConfigureUserRole(builder);
            ConfigureUserToken(builder);

            #endregion
        }

        #region Global

        /// <summary>
        /// This method sets the filters to every query 
        /// on the entity that implements IBaseEntity interface
        /// Although this method sets the query filter, if you explicitly want
        /// to add new filters, keep in mind that you are overriding these filters
        /// see more: https://github.com/dotnet/efcore/issues/10275
        /// </summary>
        /// <param name="builder"></param>
        void SetGlobalFilters(ModelBuilder builder)
        {
            //IsDeleted filter for soft deletable entities
            builder.ApplyGlobalFilters<IBaseSoftDeletableEntity>(entity => !entity.IsDeleted);

            //Filter for Company related entities
            //More levels == worse performance
            builder.ApplyGlobalFilters<IBaseSoftDeletableCompanyEntity>(entity =>
            !entity.IsDeleted &&
            //All
            ((_currentCompany.Id() == null && entity.Company.AccountType == AccountType.Regular) ||
            //Level 0 company
            entity.CompanyId == _currentCompany.Id() ||
            //Other levels
            Set<Company>().IgnoreQueryFilters().Any(y => y.Id == entity.CompanyId && (
                //Level 1 company
                (y.Id == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.AccountType == AccountType.Regular)) ||
                //Level 2 company
                (y.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.AccountType == AccountType.Regular)) ||
                //Level 3 company
                (y.Company.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.Company.AccountType == AccountType.Regular)) ||
                //Level 4 company
                (y.Company.Company.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.Company.Company.AccountType == AccountType.Regular))
            //...
            ))
            ));

            //Filter for Companies
            //More levels == worse performance
            builder.ApplyGlobalFilters<Company>(entity =>
            !entity.IsDeleted &&
            //All
            ((_currentCompany.Id() == null && entity.AccountType == AccountType.Regular) ||
            //Level 0 company
            (_currentCompany.Id() != null && entity.CompanyId == _currentCompany.Id()) ||
            //Other levels
            Set<Company>().IgnoreQueryFilters().Any(y => y.Id == entity.CompanyId && (
                //Level 1 company
                (y.Id == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.AccountType == AccountType.Regular)) ||
                //Level 2 company
                (_currentCompany.Id() != null && y.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.AccountType == AccountType.Regular)) ||
                //Level 3 company
                (_currentCompany.Id() != null && y.Company.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.Company.AccountType == AccountType.Regular)) ||
                //Level 4 company
                (_currentCompany.Id() != null && y.Company.Company.CompanyId == _currentCompany.Id() && (_currentBaseCompany.IsGuest() || y.Company.Company.Company.AccountType == AccountType.Regular))
            //...
            ))
            ));
        }

        void SetGlobalCascadeDelete(ModelBuilder builder)
        {
            var fks = builder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(fk => !fk.IsOwnership);

            foreach (var fk in fks)
                fk.DeleteBehavior = DeleteBehavior.Cascade;
        }

        #endregion

        #region SaveChanges 

        private DatabaseContextOptions _options = new DatabaseContextOptions();

        /// <summary>
        /// Allows the user to disable automatic attaching of the 
        /// CompanyId while persisting changes to the database for
        /// all entities that implement IBaseCompanyEntity
        /// </summary>
        /// <returns>DatabaseContext</returns>
        public DatabaseContext IgnoreCompanyStamps()
        {
            _options.IgnoreCompanyStampsOnSaveChanges = true;
            return this;
        }

        /// <summary>
        /// Allows the user to disable automatic attaching of the 
        /// DateTime stamps while persisting changes to the database for
        /// all entities that implement IBaseAuditableEntity
        /// </summary>
        /// <returns>DatabaseContext</returns>
        public DatabaseContext IgnoreAuditableStamps()
        {
            _options.IgnoreAuditableStampsOnSaveChanges = true;
            return this;
        }

        /// <summary>
        /// Allows the user to disable automatic attaching of the 
        /// DateTime stamps and IsDeleted value while persisting changes 
        /// to the database for all entities that implement IBaseSoftDeletableEntity
        /// </summary>
        /// <returns>DatabaseContext</returns>
        public DatabaseContext IgnoreSoftDeletableStamps()
        {
            _options.IgnoreSoftDeletableStampsOnSaveChanges = true;
            return this;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity || x.Entity is IBaseEntity<string>);

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                    HandleEntityStateAdded(entity);
                else if (entity.State == EntityState.Modified)
                    HandleEntityStateModified(entity);
                else if (entity.State == EntityState.Deleted)
                    HandleEntityStateDeleted(entity);
            }

            _options.Reset();
        }

        #region Handle EntityState Added

        private void HandleEntityStateAdded(EntityEntry entity)
        {
            if (entity.Entity is IBaseAuditableEntity && !_options.IgnoreAuditableStampsOnSaveChanges)
            {
                ((IBaseAuditableEntity)entity.Entity).CreatedAtUtc = DateTime.UtcNow;
            }
            if (entity.Entity is IBaseCompanyEntity && !_options.IgnoreCompanyStampsOnSaveChanges)
            {
                ((IBaseCompanyEntity)entity.Entity).CompanyId ??= _currentCompany.Id();
            }
        }

        #endregion

        #region Handle EntityState Modified

        private void HandleEntityStateModified(EntityEntry entity)
        {
            if (entity.Entity is IBaseAuditableEntity && !_options.IgnoreAuditableStampsOnSaveChanges)
            {
                ((IBaseAuditableEntity)entity.Entity).ModifiedAtUtc = DateTime.UtcNow;
            }
        }

        #endregion

        #region Handle EntityState Deleted

        private void HandleEntityStateDeleted(EntityEntry entity)
        {
            if (entity.Entity is IBaseSoftDeletableEntity && !_options.IgnoreSoftDeletableStampsOnSaveChanges)
            {
                entity.State = EntityState.Modified;
                ((IBaseSoftDeletableEntity)entity.Entity).IsDeleted = true;
            }
            else if (entity.Entity is IBaseSoftDeletableAuditableEntity && !_options.IgnoreSoftDeletableStampsOnSaveChanges)
            {
                entity.State = EntityState.Modified;
                ((IBaseSoftDeletableAuditableEntity)entity.Entity).IsDeleted = true;
                ((IBaseSoftDeletableAuditableEntity)entity.Entity).DeletedAtUtc = DateTime.UtcNow;
            }

            if (!_options.IgnoreSoftDeletableStampsOnSaveChanges)
                HandleCascadeSoftDelete(entity);
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
                                HandleEntityStateDeleted(collectionEntry.FindEntry(dependentEntry));
                            }
                        }
                    }
                }
                else
                {
                    if (navigationEntry.CurrentValue != null)
                    {
                        HandleEntityStateDeleted(navigationEntry.EntityEntry);
                    }
                }
            }
        }

        #endregion 

        #endregion

        #endregion
    }
}
