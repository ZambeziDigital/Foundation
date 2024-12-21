using Microsoft.AspNetCore.Components;

namespace ZambeziDigital.Blazor.Components.Models;

public class MenuOption<T> where T : class, new()
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<MenuOption<T>>? SubMenu { get; set; } = new();
    public Func<T, Task>? OnClick { get; set; }
    public bool Navigate { get; set; } = false;
}

public class MenuOption : MenuOption<object>;


