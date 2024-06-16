namespace Hubex.Module.Work.Services.Interfaees;

public interface IWorkTaskUserCacheService
{
    Task TaskUserCacheAggregateAsync(short tenantId);
    Task TaskUserCacheAggregateResponsibilityAsync(short tenantId);
}