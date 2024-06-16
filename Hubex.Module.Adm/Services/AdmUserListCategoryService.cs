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
       var userRoles = await _context.UserRoles
          .Where(ur => ur.TenantId == tenantId && ur.Deleted == null)
          .ToListAsync();

       var rolePermissions = await _context.RolePermissions
          .Where(rpe => rpe.TenantId == tenantId && rpe.Deleted == null)
          .ToListAsync();

       var users = await _context.Users
          .Include(u => u.UserRoles)
          .ToListAsync();

       var listCategories = await _context.ListCategories.ToListAsync();

       var result = users.SelectMany(u => listCategories.Where(lc =>
             userRoles.Any(ur => ur.UserId == u.Id &&
                                 rolePermissions.Any(rpe => rpe.RoleId == ur.RoleId &&
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