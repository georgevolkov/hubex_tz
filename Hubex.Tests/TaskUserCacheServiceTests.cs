using Hubex.Module.Adm.Data;
using Hubex.Module.Adm.Models;
using Hubex.Module.Adm.Services;
using Hubex.Module.Adm.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Hubex.Tests;

public class TaskUserCacheServiceTests
{
   [Theory]
   [InlineData(1, 1, 1)]
   [InlineData(3, 3, 0)]
   public async Task AggregateTaskUserCacheAsync(
      short tenantId,
      byte districtAvailable,
      int expectedRecordsCount)
   {
      // Arrange
      var options = new DbContextOptionsBuilder<AdmDbContext>()
         .UseInMemoryDatabase(databaseName: "TestDatabase")
         .Options;

      await CleanContextBeforeTest(options);

      await using (var context = new AdmDbContext(options))
      {
         context.UserTaskListCategories.Add(new UserTaskListCategory { UserId = 1, TaskListCategoryId = 1 });
         context.TaskResponsibleUsers.Add(new TaskResponsibleUser { TaskId = 1, UserId = 1 });
         context.UserDistricts.Add(new UserDistrict { TenantId = 1, UserId = 1, DistrictId = 1, Deleted = null });

         await context.SaveChangesAsync();
      }

      await using (var context = new AdmDbContext(options))
      {
         var mockLogger = new Mock<ILogger<AdmTaskUserCacheService>>();
         var mockPredicateExtensions = new PredicateExtensions(context);

         var service = new AdmTaskUserCacheService(context, mockPredicateExtensions, mockLogger.Object);

         // Act
         await service.AggregateTaskUserCacheAsync(tenantId, districtAvailable);

         // Assert
         var taskUserCaches = await context.TaskUserCaches
            .Where(tuc => tuc.TaskListCategoryId == districtAvailable)
            .ToListAsync();

         Assert.Equal(expectedRecordsCount, taskUserCaches.Count);
      }
   }

   private static async Task CleanContextBeforeTest(DbContextOptions<AdmDbContext> options)
   {
      await using (var context = new AdmDbContext(options))
      {
         context.UserTaskListCategories.RemoveRange(context.UserTaskListCategories);
         context.TaskResponsibleUsers.RemoveRange(context.TaskResponsibleUsers);
         context.UserDistricts.RemoveRange(context.UserDistricts);
         await context.SaveChangesAsync();
      }
   }
}
