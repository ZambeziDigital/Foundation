global using System.ComponentModel.DataAnnotations.Schema;

namespace ZambeziDigital.BasicAccess.Models;

public class Address : BaseModel
{
     public string? FirstName { get; set; } = string.Empty;
     public string? LastName { get; set; } = string.Empty;
     [NotMapped] public string? Name  => $"{Street}, {City}, {Province}";
     public string? Street { get; set; } = string.Empty;
     public string? City { get; set; } = string.Empty;
     public string? Province { get; set; } = string.Empty;
     public string? State { get; set; } = string.Empty;
     public string? LineOne { get; set; } = string.Empty;
     public string? LineTwo { get; set; } = string.Empty;
     public string? ZIP { get; set; } = string.Empty;
}

