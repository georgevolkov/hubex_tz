namespace Hubex.Module.Adm.Models;

public class UserDistrict
{
    public int       Id         { get; set; }
    public int       TenantId   { get; set; }
    public int       UserId     { get; set; }
    public int       DistrictId { get; set; }
    public DateTime? Deleted    { get; set; }
}