using Hubex.Module.Adm.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubex.Module.Adm.Data;

public class AdmDbContext : DbContext
{
   public DbSet<TaskResponsibleUser>  TaskResponsibleUsers   { get; set; }
   public DbSet<UserTaskListCategory> UserTaskListCategories { get; set; }
   public DbSet<TaskUserCache>        TaskUserCaches         { get; set; }
   public DbSet<UserDistrict>         UserDistricts          { get; set; }
   public DbSet<User>                 Users                  { get; set; }
   public DbSet<ListCategory>         ListCategories         { get; set; }
   public DbSet<UserRole>             UserRoles              { get; set; }
   public DbSet<RolePermissionExt>    RolePermissions        { get; set; }

   public AdmDbContext(DbContextOptions<AdmDbContext> options) : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<UserDistrict>()
         .HasKey(ud => new { ud.TenantId, ud.UserId, ud.DistrictId });

      modelBuilder.Entity<TaskResponsibleUser>()
         .HasKey(f => f.Id);
      modelBuilder.Entity<UserTaskListCategory>()
         .HasKey(f => f.Id);
      modelBuilder.Entity<TaskUserCache>()
         .HasKey(f => f.Id);

      modelBuilder.Entity<User>().HasKey(u => u.Id);
      modelBuilder.Entity<ListCategory>().HasKey(lc => lc.Id);
      modelBuilder.Entity<UserRole>().HasKey(ur => ur.Id);
      modelBuilder.Entity<RolePermissionExt>().HasKey(rpe => rpe.Id);

      modelBuilder.Entity<UserRole>()
         .HasOne(ur => ur.User)
         .WithMany(u => u.UserRoles)
         .HasForeignKey(ur => ur.UserId);

      modelBuilder.Entity<UserRole>()
         .HasMany(ur => ur.RolePermissions)
         .WithOne()
         .HasForeignKey(rpe => rpe.RoleId);


      Seed(modelBuilder);
   }

   private void Seed(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<TaskResponsibleUser>().HasData(
         new TaskResponsibleUser { Id = 1, TaskId = 1, UserId = 1 },
         new TaskResponsibleUser { Id = 2, TaskId = 2, UserId = 2 }
      );

      modelBuilder.Entity<UserTaskListCategory>().HasData(
         new UserTaskListCategory { Id = 1, UserId = 1, TaskListCategoryId = 1 },
         new UserTaskListCategory { Id = 2, UserId = 2, TaskListCategoryId = 2 }
      );

      modelBuilder.Entity<UserDistrict>().HasData(
         new UserDistrict { Id = 1, TenantId = 1, UserId = 1, DistrictId = 1 },
         new UserDistrict { Id = 2, TenantId = 1, UserId = 2, DistrictId = 2 }
      );

      modelBuilder.Entity<RolePermissionExt>().HasData(
         new RolePermissionExt { Id = 1, TenantId = 1, RoleId = 1, PermissionExtId = 1 },
         new RolePermissionExt { Id = 2, TenantId = 1, RoleId = 2, PermissionExtId = 2 }
      );

      modelBuilder.Entity<UserRole>().HasData(
         new UserRole { Id = 1, TenantId = 1, UserId = 1, RoleId = 1 },
         new UserRole { Id = 2, TenantId = 1, UserId = 2, RoleId = 1 }
      );

      modelBuilder.Entity<User>().HasData(
         new User { Id = 1, UserRoles = new List<UserRole>() },
         new User { Id = 2, UserRoles = new List<UserRole>() }
      );

      modelBuilder.Entity<ListCategory>().HasData(
         new ListCategory { Id = 1, PermissionExtId = 1 },
         new ListCategory { Id = 2, PermissionExtId = 2 }
      );
   }
}