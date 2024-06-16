using System.Text.Json.Serialization;

namespace Hubex.Module.Adm.Models;

public class UserTaskListCategory
{
    [JsonIgnore]
    public int Id     { get; set; }
    public int UserId { get; set; }
    public int TaskListCategoryId { get; set; }
}