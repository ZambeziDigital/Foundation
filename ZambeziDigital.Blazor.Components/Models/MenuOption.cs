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

public static class TypeExtensions
{
    public static bool IsNumericType(this Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }
    }
}


