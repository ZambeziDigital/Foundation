using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ZambeziDigital.Functions.Helpers;

namespace ZambeziDigital.Base.Models;

public class Address : BaseModel<int>
{
    [NotMapped]
    public override string Name
    {
        get
        {
            var result = "";
            if (!string.IsNullOrEmpty(Street))
            {
                result += Street;
            }

            if (!string.IsNullOrEmpty(City))
            {
                result += $", {City}";
            }

            if (!string.IsNullOrEmpty(Province))
            {
                result += $", {Province}";
            }

            return result;
        }
    }

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? Street { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? LastName { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? FirstName { get; set; } = string.Empty;

    public virtual string? State { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? City { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? ZIP { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? Province { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? Description { get; set; } = string.Empty;

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? LineOne { get; set; }

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? Country { get; set; }

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? LineTwo { get; set; }

    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public virtual string? HouseNumber { get; set; }



    public override string ToString() => Name;
}