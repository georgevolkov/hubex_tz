using Hubex.Module.Work.Data;
using Hubex.Module.Work.Models;
using Hubex.Module.Work.Services.Interfaees;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Hubex.Module.Work.Services;

public class WorkTaskUserCacheService : IWorkTaskUserCacheService
{
   private readonly WorkDbContext _context;

   public WorkTaskUserCacheService(WorkDbContext context)
   {
      _context = context;
   }

   public async Task TaskUserCacheAggregateAsync(short tenantId)
   {
      byte assignedTo = 2;
      byte districtAvailable = 13;

      var taskAssigned = await _context.TaskOnlineAssigneds
         .Where(toa => toa.TenantId == tenantId && _context.Tasks.Any(t => t.Id == toa.TaskId))
         .Select(toa => new TaskAssigned
         {
            TaskId = toa.TaskId,
            AssignedTo = toa.AssignedTo
         })
         .ToListAsync();

      var taskUserCache = taskAssigned
         .Where(ta => ta.AssignedTo.HasValue && _context.Users.Any(u => u.Id == ta.AssignedTo.Value))
         .Select(ta => new TaskUserCache
         {
            TaskId = ta.TaskId,
            UserId = ta.AssignedTo.Value,
            TaskListCategoryId = assignedTo
         })
         .ToList();

      await _context.TaskUserCaches.AddRangeAsync(taskUserCache);
      await _context.SaveChangesAsync();

      var taskResponsibleUser = new List<TaskResponsibleUser>();

      if (_context.UserTaskListCategories.Any(utlc => utlc.TaskListCategoryId == districtAvailable))
      {
         var distinctTaskUserPairs = taskAssigned
            .SelectMany(ta => new[] { ta.Task.ApprovalWith, ta.Task.EscalatedTo, ta.AssignedTo })
            .Where(userId => userId.HasValue)
            .Select(userId => (taskId: userId.Value, userId: userId.Value))
            .Distinct()
            .ToList();

         taskResponsibleUser.AddRange(distinctTaskUserPairs
            .Select(pair => new TaskResponsibleUser
            {
               TaskId = pair.taskId,
               UserId = pair.userId
            }));
      }

      await _context.TaskResponsibleUsers.AddRangeAsync(taskResponsibleUser);
      await _context.SaveChangesAsync();
   }

   // TODO тут надо схему БД оптимизировать чтобы понимать как лучше запросы строить
   public async Task TaskUserCacheAggregateResponsibilityAsync (short tenantId)
   {
      const byte AssignedTo = 2;
      const byte DistrictAvailable = 13;

      var taskAssigned = await _context.TaskOnlineAssigneds
         .Where(toa => toa.TenantId == tenantId && toa.AssignedTo.HasValue)
         .Select(toa => new TaskAssigned { TaskId = toa.TaskId, AssignedTo = toa.AssignedTo })
         .ToListAsync();

      var tasks = await _context.Tasks
         .Where(t => taskAssigned.Select(ta => ta.TaskId).Contains(t.Id))
         .ToListAsync();

      var users = await _context.Users
         .Where(u => taskAssigned.Select(ta => ta.AssignedTo.Value).Contains(u.Id))
         .ToListAsync();

      var userTaskListCategories = await _context.UserTaskListCategories
         .Where(utlc => utlc.TaskListCategoryId == DistrictAvailable)
         .ToListAsync();

      var taskUserCaches = taskAssigned
         .Where(ta => users.Any(u => u.Id == ta.AssignedTo))
         .Select(ta => new TaskUserCache
         {
            TaskId = ta.TaskId,
            UserId = ta.AssignedTo.Value,
            TaskListCategoryId = AssignedTo
         }).ToList();

      await _context.TaskUserCaches.AddRangeAsync(taskUserCaches);
      await _context.SaveChangesAsync();

      if (userTaskListCategories.Any())
      {
         var taskResponsibleUsers = tasks
            .Where(t => t.ApprovalWith.HasValue || t.EscalatedTo.HasValue)
            .SelectMany(t => new[]
            {
               new TaskResponsibleUser { TaskId = t.Id, UserId = t.ApprovalWith ?? 0 },
               new TaskResponsibleUser { TaskId = t.Id, UserId = t.EscalatedTo ?? 0 }
            })
            .Where(tru => tru.UserId != 0)
            .Union(taskAssigned.Select(ta => new TaskResponsibleUser
            {
               TaskId = ta.TaskId,
               UserId = ta.AssignedTo.Value
            }))
            .Distinct()
            .ToList();

         await _context.TaskResponsibleUsers.AddRangeAsync(taskResponsibleUsers);
         await _context.SaveChangesAsync();
      }
   }
}