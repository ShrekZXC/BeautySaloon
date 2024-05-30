using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BeautySaloon.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.DAL
{
    public class BeautySaloonDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public BeautySaloonDbContext(DbContextOptions<BeautySaloonDbContext> options) : base(options)
        {
        }
        
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<PromotionEntity> Promotions { get; set; }
        public DbSet<WorkScheduletEntity> WorkSchedules { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ServiceEntity>().ToTable("Service");
            modelBuilder.Entity<CategoryEntity>().ToTable("Category");
            modelBuilder.Entity<PromotionEntity>().ToTable("Promotion");
            modelBuilder.Entity<WorkScheduletEntity>().ToTable("WorkSchedule");
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public Microsoft.EntityFrameworkCore.DbSet<T> DbSet<T>() where T : class
        {
            return Set<T>();
        }

        public new IQueryable<T> Query<T>() where T : class
        {
            return Set<T>();
        }
    }

    public class DbSet<T>
    {
    }
}