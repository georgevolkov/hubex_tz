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

      if (!userTaskListCategories.Any())
      {
         _logger.LogInformation("No UserTaskListCategories found for districtAvailable {districtAvailable}",
            districtAvailable);
         return;
      }

      var taskUserCaches = await _context.TaskResponsibleUsers
         .Join(_context.UserDistricts,
            ur => ur.UserId,
            ud => ud.UserId,
            (ur, ud) => new { ur, ud })
         .Where(joined => joined.ud.TenantId == tenantId && joined.ud.Deleted == null)
         .Where(joined => _context.UserDistricts
            .Any(ud1 => ud1.TenantId == tenantId &&
                        ud1.DistrictId == joined.ud.DistrictId &&
                        ud1.UserId == joined.ur.UserId &&
                        ud1.Deleted == null))
         .Join(userTaskListCategories,
            joined => joined.ud.UserId,
            utlc => utlc.UserId,
            (joined, utlc) => new TaskUserCache
            {
               TaskId = joined.ur.TaskId,
               UserId = utlc.UserId,
               TaskListCategoryId = utlc.TaskListCategoryId
            })
         .ToListAsync();

      if (taskUserCaches.Any())
      {
         await _context.TaskUserCaches.AddRangeAsync(taskUserCaches);
         await _context.SaveChangesAsync();
         _logger.LogInformation("{taskUserCachesCount} task saved!", taskUserCaches.Count);
      }
      else
      {
         _logger.LogInformation("No TaskUserCaches to save.");
      }
   }
}