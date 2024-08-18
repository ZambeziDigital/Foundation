namespace ZambeziDigital.Base.Contracts.Base;

public interface IAddress : IBaseModel<int>
{
     public string? FirstName { get; set; }
     public string? LastName { get; set; }
     public string? Name  => $"{Street}, {City}, {Province}";
     public string? Street { get; set; }
     public string? City { get; set; }
     public string? Province { get; set; }
     public string? State { get; set; }
     public string? LineOne { get; set; }
     public string? LineTwo { get; set; }
     public string? ZIP { get; set; }
}

