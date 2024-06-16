using Hubex.Module.Adm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hubex.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdmSchemeController : ControllerBase
{
   private readonly IAdmTaskUserCacheService _admTaskUserCacheService;
   private readonly IAdmUserListCategoryService _admUserCategoryService;
   private readonly ILogger<AdmSchemeController> _logger;

   public AdmSchemeController(
      IAdmTaskUserCacheService admTaskUserCacheService,
      IAdmUserListCategoryService admUserCategoryService,
      ILogger<AdmSchemeController> logger)
   {
      _admTaskUserCacheService = admTaskUserCacheService;
      _admUserCategoryService = admUserCategoryService;
      _logger = logger;
   }

   [HttpGet("AggregateUserTasks")]
   public async Task<IActionResult> AggregateUserTasks(short tenantId, byte districtAvailable)
   {
      _logger.LogInformation("Call AggregateUserTasks...");
      await _admTaskUserCacheService.AggregateTaskUserCacheAsync(tenantId, districtAvailable);
      return Ok("User tasks aggregated successfully!");
   }

   [HttpGet("GetUserListCategoriesAsync")]
   public async Task<IActionResult> GetUserListCategoriesAsync(short tenantId)
   {
      _logger.LogInformation ("Call GetUserListCategoriesAsync...");
      return Ok(await _admUserCategoryService.GetUserListCategoriesAsync(tenantId));
   }
}