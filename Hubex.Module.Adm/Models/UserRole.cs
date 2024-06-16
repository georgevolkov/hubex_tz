namespace Hubex.Module.Adm.Models;

public class UserRole
{
   public int                            Id              { get; set; }
   public int                            TenantId        { get; set; }
   public int                            UserId          { get; set; }
   public int                            RoleId          { get; set; }
   public bool?                          Deleted         { get; set; }
   public User                           User            { get; set; }
   public ICollection<RolePermissionExt> RolePermissions { get; set; }
}