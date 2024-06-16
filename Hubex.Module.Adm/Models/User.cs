namespace Hubex.Module.Adm.Models;

public class User
{
   public int                   Id        { get; set; }
   public ICollection<UserRole> UserRoles { get; set; }
}