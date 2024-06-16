using Hubex.Module.Work.Services.Interfaees;
using Microsoft.AspNetCore.Mvc;

namespace Hubex.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkSchemeController : ControllerBase
{
   private readonly IWorkTaskUserCacheService _workTaskService;
   private readonly ILogger<WorkSchemeController> _logger;

   public WorkSchemeController(
      IWorkTaskUserCacheService workTaskService,
      ILogger<WorkSchemeController> logger)
   {
      _workTaskService = workTaskService;
      _logger = logger;
   }

   [HttpGet("TaskUserCacheAggregate")]
   public async Task<IActionResult> TaskUserCacheAggregate(short tenantId)
   {
      _logger.LogInformation("Call TaskUserCacheAggregate...");
      await _workTaskService.TaskUserCacheAggregateAsync(tenantId);
      return Ok("User tasks aggregated successfully!");
   }

   [HttpGet("TaskUserCacheAggregateResponsibility")]
   public async Task<IActionResult> TaskUserCacheAggregateResponsibility(short tenantId)
   {
      _logger.LogInformation ("Call TaskUserCacheAggregateResponsibility...");
      await _workTaskService.TaskUserCacheAggregateResponsibilityAsync(tenantId);
      return Ok("User tasks responsibility aggregated successfully!");
   }
}