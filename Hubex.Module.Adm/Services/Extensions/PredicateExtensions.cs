using System.Linq.Expressions;
using Hubex.Module.Adm.Data;
using Hubex.Module.Adm.Models;

namespace Hubex.Module.Adm.Services.Extensions;

public class PredicateExtensions
{
    private readonly AdmDbContext _db;

    public PredicateExtensions(AdmDbContext db)
    {
        _db = db;
    }

    public Expression<Func<UserDistrict, bool>> HasUserDistrictRelation(int tenantId, UserTaskListCategory? taskListCategory, TaskResponsibleUser? responsibleUser)
    {
        if (taskListCategory == null || responsibleUser == null)
            return district => false;

        return ud => ud.TenantId == tenantId &&
                     ud.UserId == taskListCategory.UserId &&
                     ud.Deleted == null &&
                     _db.UserDistricts.Any(ud1 => ud1.UserId == ud.UserId &&
                                                  ud1.UserId == ud.UserId &&
                                                  ud1.UserId == responsibleUser.UserId && ud1.Deleted == null);
    }
}