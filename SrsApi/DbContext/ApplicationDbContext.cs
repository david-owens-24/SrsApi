using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.OpenApi.Extensions;
using SrsApi.DbModels;
using SrsApi.Enums;
using SrsApi.Interfaces;
using System.Reflection.Emit;

namespace SrsApi.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private IUserResolutionService _userResolutionService;

        public DbSet<SrsAnswer> SrsAnswers { get; set; }
        public DbSet<SrsAnswerFuzzySearchMethod> SrsAnswerFuzzySearchMethods { get; set; }
        public DbSet<SrsFuzzySearchMethod> SrsFuzzySearchMethods { get; set; }
        public DbSet<SrsItem> SrsItems { get; set; }
        public DbSet<SrsItemDetails> SrsItemDetails { get; set; }
        public DbSet<SrsItemLevel> SrsItemLevels { get; set; }
        public DbSet<Error> Errors { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserResolutionService userResolutionService) :
            base(options)
        {
            _userResolutionService = userResolutionService;
            this.ChangeTracker.StateChanged += this.UpdateBaseEntity;
            this.ChangeTracker.Tracked += this.UpdateBaseEntity;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed roles
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "44d79373-d381-42da-9e71-92e8e48abf60", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });

            //seed SrsFuzzySearchMethods by looping through the FuzzySearchMethod enum
            foreach(var fuzzySearchMethod in (FuzzySearchMethod[])Enum.GetValues(typeof(FuzzySearchMethod)))
            {
                modelBuilder.Entity<SrsFuzzySearchMethod>().HasData(new SrsFuzzySearchMethod { FuzzySearchMethod = fuzzySearchMethod, FuzzySearchMethodName = fuzzySearchMethod.ToString() });
            }
        }

        private void UpdateBaseEntity(object sender, EntityEntryEventArgs e)
        {
            //sets the Added or Modified date automatically on update

            if (e.Entry.Entity is BaseDbModel baseEntity)
            {
                if (e.Entry.State == EntityState.Added)
                {
                    // Only set values if not set manually
                    baseEntity.CreatedBy ??= _userResolutionService.GetCurrentSessionUserId(this);
                    baseEntity.Created ??= DateTime.Now;
                }
                else if (e.Entry.State == EntityState.Modified)
                {
                    //TODO: add modified date/modified by?

                    var entity = (BaseDbModel)e.Entry.Entity;

                    if (entity.Deleted != null && entity.DeletedBy == null)
                    {
                        baseEntity.DeletedBy ??= _userResolutionService.GetCurrentSessionUserId(this);
                    }
                }
                else
                {
                    //do nothing
                }
            }
        }
    }
}
