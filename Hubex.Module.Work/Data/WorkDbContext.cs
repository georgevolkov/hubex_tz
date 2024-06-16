using Hubex.Module.Work.Models;
using Microsoft.EntityFrameworkCore;
using Task = Hubex.Module.Work.Models.Task;

namespace Hubex.Module.Work.Data;

public class WorkDbContext : DbContext
{
   public DbSet<Task>                 Tasks                  { get; set; }
   public DbSet<User>                 Users                  { get; set; }
   public DbSet<TaskResponsibleUser>  TaskResponsibleUsers   { get; set; }
   public DbSet<TaskUserCache>        TaskUserCaches         { get; set; }
   public DbSet<TaskOnlineAssigned>   TaskOnlineAssigneds    { get; set; }
   public DbSet<UserTaskListCategory> UserTaskListCategories { get; set; }

   public WorkDbContext (DbContextOptions<WorkDbContext> options) : base (options) { }

   protected override void OnModelCreating (ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Task> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<TaskOnlineAssigned> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<TaskAssigned> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<TaskResponsibleUser> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<TaskUserCache> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<User> ()
         .HasKey (f => f.Id);
      modelBuilder.Entity<UserTaskListCategory> ()
         .HasKey (f => f.Id);

      base.OnModelCreating (modelBuilder);
   }
}