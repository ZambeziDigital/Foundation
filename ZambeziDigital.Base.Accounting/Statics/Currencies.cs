namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<Currency> Currencies = new List<Currency>
    {
        new Currency {Code = "ZMW", Id = 1, Name = "Zambian kwacha", Description = "Zambian kwacha"},
        new Currency {Code = "USD", Id = 2, Name = "United States Dollar", Description = "United States Dollar"},
        new Currency {Code = "ZAR", Id = 3, Name = "South African Rand", Description = "South African Rand"},
        new Currency {Code = "GBP", Id = 4, Name = "Pound Sterling", Description = "Pound Sterling"},
        new Currency {Code = "CNY", Id = 5, Name = "Chinese Yuan", Description = "Chinese Yuan"},
        new Currency {Code = "EUR", Id = 6, Name = "Euro", Description = "Euro"}
    };
}