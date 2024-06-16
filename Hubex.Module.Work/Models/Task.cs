namespace Hubex.Module.Work.Models;

public class Task
{
   public int       Id           { get; set; }
   public int?      ApprovalWith { get; set; }
   public int?      EscalatedTo  { get; set; }
}