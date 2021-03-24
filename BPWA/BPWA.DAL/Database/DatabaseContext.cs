using BPWA.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BPWA.DAL.Database
{
    public class DatabaseContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<BusinessUnitUser> BusinessUnitUsers { get; set; }
        public DbSet<BusinessUnitUserRole> BusinessUnitUserRoles { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<CompanyUserRole> CompanyUserRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryCurrency> CountryCurrencies { get; set; }
        public DbSet<CountryLanguage> CountryLanguages { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

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

        #endregion Configure Identity entities

        #region Helpers 

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureRole(builder);
            ConfigureRoleClaim(builder);
            ConfigureUser(builder);
            ConfigureUserClaim(builder);
            ConfigureUserLogin(builder);
            ConfigureUserRole(builder);
            ConfigureUserToken(builder);
        }


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
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IBaseEntity)entity.Entity).CreatedAtUtc = DateTime.UtcNow;
                }
                if (entity.State == EntityState.Modified)
                {
                    if (((IBaseEntity)entity.Entity).IsDeleted)
                    {
                        ((IBaseEntity)entity.Entity).DeletedAtUtc = DateTime.UtcNow;
                    }
                    else
                    {
                        ((IBaseEntity)entity.Entity).ModifiedAtUtc = DateTime.UtcNow;
                    }
                }
            }
        }

        #endregion
    }
}
