namespace Hubex.Module.Adm.Services.Interfaces;

public interface IAdmTaskUserCacheService
{
    Task AggregateTaskUserCacheAsync(short tenantId, byte districtAvailable);
}
