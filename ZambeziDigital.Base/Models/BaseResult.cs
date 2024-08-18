
namespace ZambeziDigital.Base.Models;

public class BaseResult<T> where T : class
{
    public List<string> Errors { get; set; } = new List<string>();
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; } = true;
    public T? Data { get; set; } = null;
    public ListDetails? ListDetails { get; set; }
}

public class ListDetails
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}

public class BaseResult : BaseResult<object>
{
}

