namespace Hubex.Module.Work.Models;

public class TaskUserCache
{
   public int  Id                 { get; set; }
   public int  TaskId             { get; set; }
   public int? UserId             { get; set; }
   public byte TaskListCategoryId { get; set; }
}