namespace Hubex.Module.Work.Models;

public class TaskAssigned
{
   public         int  Id         { get; set; }
   public         int  TaskId     { get; set; }
   public virtual Task Task       { get; set; }
   public         int? AssignedTo { get; set; }
}