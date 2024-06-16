namespace Hubex.Module.Adm.Models;

public class RolePermissionExt
{
   public int   Id              { get; set; }
   public int   TenantId        { get; set; }
   public int   RoleId          { get; set; }
   public int   PermissionExtId { get; set; }
   public bool? Deleted         { get; set; }
}