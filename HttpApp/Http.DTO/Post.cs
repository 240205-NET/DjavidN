using System.Data.Common;
using System.Dynamic;

namespace Http.DTO;

public class Post
{
  public int? userId { get;set; }
  public int? id { get; set; }
  public string? title { get;set; }
  public string? body { get;set; }

}
