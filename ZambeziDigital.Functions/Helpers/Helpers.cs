using System.Reflection;
using System.Text.RegularExpressions;

namespace ZambeziDigital.Functions.Helpers;

public static class Helpers
{
    public static string ConvertCamelCaseToReadable(string str)
    {
        return Regex.Replace(str, "(\\B[A-Z])", " $1");
    }
    
    public class DigitalProperty
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object Object { get; set; }
        public string Name { get => ConvertCamelCaseToReadable(PropertyInfo.Name); }
        public string PropertyName { get => PropertyInfo.Name; }
        public bool Value { get => (bool)PropertyInfo.GetValue(Object); set => PropertyInfo.SetValue(Object, value); }
    }
  
    public static List<DigitalProperty> GetProperties(object Group, Type? type = null)
    {
        var result = Group is not null ? Group
            .GetType()
            .GetProperties()
            .Select(p => new DigitalProperty {  Object = Group, PropertyInfo = p }).ToList() : new();
        if(type is not null)
        {
            result = result.Where(p => p.PropertyInfo.PropertyType == type).ToList();
        }

        foreach (var prop in result)
        {
            result = result.Where(p => p.PropertyInfo.GetCustomAttribute<DigitalColumn>() is not null).ToList();
        }

        return result;
    }
    
    public static List<DigitalProperty> GetProperties<TAttribute>(object Group, Type? type = null) where TAttribute : Attribute
    {
        var result = Group is not null ? Group
            .GetType()
            .GetProperties()
            .Select(p => new Helpers.DigitalProperty {  Object = Group, PropertyInfo = p }).ToList() : new();
        if(type is not null)
        {
            result = result.Where(p => p.PropertyInfo.PropertyType == type).ToList();
        }

        foreach (var prop in result)
        {
            result = result.Where(p => p.PropertyInfo.GetCustomAttribute<TAttribute>() is not null).ToList();
        }

        return result;
    }

   
}

[AttributeUsage(AttributeTargets.All)]
public class DigitalColumn() : Attribute;
    
[AttributeUsage(AttributeTargets.All)]
public class PassOnCreate() : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class Searchable : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public class DigitalDetail : Attribute;