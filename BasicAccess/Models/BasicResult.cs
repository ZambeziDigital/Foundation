namespace ZambeziDigital.BasicAccess.Models;
public class BasicResult<T> where T : class
{
    public List<string>? Errors { get; set; } = new();
    public bool Succeeded { get; set; }
    public T? Object { get; set; }
}

public class BasicResult 
{
    public List<string>? Errors { get; set; } = new();
    public bool Succeeded { get; set; }
    public object? Object { get; set; }
}