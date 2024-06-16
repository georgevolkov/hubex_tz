using Hubex.Module.Adm.Models;

namespace Hubex.Module.Adm.Services.Interfaces;

public interface IAdmUserListCategoryService
{
    Task<IEnumerable<UserTaskListCategory>> GetUserListCategoriesAsync(int tenantId);
}