using Hubex.Module.Adm.Data;
using Hubex.Module.Adm.Models;
using Hubex.Module.Adm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hubex.Module.Adm.Services;

public class AdmUserListCategoryService : IAdmUserListCategoryService
{
   private readonly AdmDbContext _context;

   public AdmUserListCategoryService(AdmDbContext context)
   {
      _context = context;
   }

   public async Task<IEnumerable<UserTaskListCategory>> GetUserListCategoriesAsync(int tenantId)
   {
      var users = await _context.Users
         .Include(u => u.UserRoles)
         .ThenInclude(r => r.RolePermissions)
         .ToListAsync();

      var listCategories = await _context.ListCategories.ToListAsync();

      var result = users.SelectMany(u => listCategories
         .Where(lc =>
            u.UserRoles.Any(ur =>
               ur.TenantId == tenantId &&
               ur.Deleted == null &&
               ur.RolePermissions.Any(rpe =>
                  rpe.TenantId == tenantId &&
                  rpe.Deleted == null &&
                  rpe.PermissionExtId == lc.PermissionExtId)) ||
            lc.PermissionExtId == null)
         .Select(lc => new UserTaskListCategory
         {
            UserId = u.Id,
            TaskListCategoryId = lc.Id
         })).ToList();

      return result;
   }
}