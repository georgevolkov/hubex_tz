namespace Hubex.Module.Work.Data;

public class TaskOnlineAssigned
   {
   public int       Id         { get; set; }
   public int       TaskId     { get; set; }
   public int?      AssignedTo { get; set; }
   public int       TenantId   { get; set; }
   public DateTime  CreatedAt  { get; set; }
   public DateTime? DeletedAt  { get; set; }
}