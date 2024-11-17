using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ZambeziDigital.AspNetCore.Abstractions.Data;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    
public class NullableUtcDateTimeConverter() : ValueConverter<DateTime?, DateTime?>(
    v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : v,
    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UtcDateTimeAttribute : Attribute;