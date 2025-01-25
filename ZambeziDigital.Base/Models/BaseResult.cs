global using System;
global using System.Collections.Generic;

namespace ZambeziDigital.Base.Models;

public class BaseResult<T> where T : class
{
    public List<string> Errors { get; set; } = new List<string>();
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; } = true;
    public T? Data { get; set; } = null;
}

public class BaseListResult<T> : BaseResult<List<T>> where T : class
{
    public required int TotalCount { get; set; }
    public  int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public required int CurrentPage { get; set; }
    public required int PageSize { get; set; }
    public  bool IsLastPage => CurrentPage >= TotalPages;
    public  bool IsFirstPage => CurrentPage == 0;
    public string? SortBy { get; set; } = string.Empty;
}






public class BaseResult : BaseResult<object>
{
}

