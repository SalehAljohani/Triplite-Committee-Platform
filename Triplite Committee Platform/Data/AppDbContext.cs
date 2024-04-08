using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.Data
{
    public class AppDbContext : IdentityDbContext<UserModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ScholarshipModel> Scholarship { get; set; }
        public DbSet<CollegeModel> College { get; set; }
        public DbSet<DepartmentModel> Department { get; set; }
        public DbSet<RequestTypeModel> RequestType { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<ReasonsModel> Reasons { get; set; }
        public DbSet<BoardModel> Board { get; set; }
        public DbSet<FileModel> File { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.EmployeeID)
                .IsUnique();

            modelBuilder.Entity<BoardModel>()
                .HasOne(b => b.Scholarship)
                .WithMany()
                .HasForeignKey(b => b.National_ID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}