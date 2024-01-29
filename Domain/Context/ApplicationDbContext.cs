using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using textileManagment.Domain.Helper;
using textileManagment.Entities;
using textileManagment.Entities.Base.IBase;

namespace textileManagment.Domain.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public virtual DbSet<AppUser> AppUsers { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var userId = HttpContextAccessor.HttpContext.GetUserId();
            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries().Where(t => t.State == EntityState.Added).Select(f => f.Entity)
                .ToArray();

            foreach (var entity in added)
            {

                switch (entity)
                {
                    case IGeneralBase addedEntity:
                        addedEntity.CreatedDate = DateTime.UtcNow;
                        addedEntity.IsDelete = false;
                        addedEntity.IsActive = true;
                        addedEntity.CreatedById = userId;
                        break;
                    case IMinActiveBase minActiveBaseEntity:
                        minActiveBaseEntity.CreatedById = userId;
                        minActiveBaseEntity.IsActive = true;
                        minActiveBaseEntity.CreatedDate = DateTime.UtcNow;
                        break;
                    case IMinBase mBAddedEntity:
                        mBAddedEntity.CreatedDate = DateTime.UtcNow;
                        break;
                }
            }

            var modified = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in modified)
            {
                if (entity is not IGeneralBase gBModifiedEntity) continue;
                gBModifiedEntity.ModifiedDate = DateTime.UtcNow;
                gBModifiedEntity.ModifiedById = userId;
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUserConfiguration>().UseTpcMappingStrategy();
            modelBuilder.SeedData();

        }
    }
}
