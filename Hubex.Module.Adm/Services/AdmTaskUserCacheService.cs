using Hubex.Module.Adm.Data;
using Hubex.Module.Adm.Models;
using Hubex.Module.Adm.Services.Extensions;
using Hubex.Module.Adm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hubex.Module.Adm.Services;

public class AdmTaskUserCacheService : IAdmTaskUserCacheService
{
   private readonly AdmDbContext _context;
   private readonly PredicateExtensions _predicateExtensions;
   private readonly ILogger<AdmTaskUserCacheService> _logger;

   public AdmTaskUserCacheService(
      AdmDbContext context,
      PredicateExtensions predicateExtensions,
      ILogger<AdmTaskUserCacheService> logger)
   {
      _context = context;
      _predicateExtensions = predicateExtensions;
      _logger = logger;
   }

   public async Task AggregateTaskUserCacheAsync(short tenantId, byte districtAvailable = 13)
   {
      var userTaskListCategories = await _context.UserTaskListCategories
         .Where(utlc => utlc.TaskListCategoryId == districtAvailable)
         .ToListAsync();

      var taskResponsibleUsers = await _context.TaskResponsibleUsers.ToListAsync();
      var taskUserCaches = new List<TaskUserCache>();

      foreach (var taskListCategory in userTaskListCategories)
      {
         foreach (var responsibleUser in taskResponsibleUsers)
         {
            var relationExists = await _context.UserDistricts
               .AnyAsync(_predicateExtensions.HasUserDistrictRelation(tenantId, taskListCategory, responsibleUser));

            if (relationExists)
            {
               taskUserCaches.Add(new TaskUserCache
               {
                  TaskId = responsibleUser.TaskId,
                  UserId = taskListCategory.UserId,
                  TaskListCategoryId = taskListCategory.TaskListCategoryId
               });
            }
         }
      }

      if (taskUserCaches.Any())
      {
         await _context.TaskUserCaches.AddRangeAsync(taskUserCaches);
         await _context.SaveChangesAsync();
         _logger.LogInformation("{taskUserCachesCount} task saved!", taskUserCaches.Count);
      }
   }
}