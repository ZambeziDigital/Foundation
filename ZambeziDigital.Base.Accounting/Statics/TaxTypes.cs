namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<TaxType> TaxTypes = new()
    {
        new TaxType
        {
            Code = "A", Id = 1, Name = "Standard Rated 16%",
            Description = "Category applies to products and services which attract VAT at 16% by nature",
            Group = TaxGroup.VAT, Rate = 16
        },
        new TaxType
        {
            Code = "B", Id = 2, Name = "Minimum Taxable Value (MTV) 16%",
            Description = "16% is charged on the Retail Price recommended by manufacturer of MTV goods.",
            Group = TaxGroup.VAT, Rate = 16
        },
        new TaxType
        {
            Code = "C1", Id = 3, Name = "Exports 0%", Description = "Applies to exports", Group = TaxGroup.VAT
        },
        new TaxType
        {
            Code = "C2", Id = 4, Name = "Zero-rating Local Purchases",
            Description =
                "0% Applies to transactions involving customers or projects granted exemption from paying taxes",
            Group = TaxGroup.VAT, Rate = 0
        },
        new TaxType
        {
            Code = "C3", Id = 5, Name = "Zero-rated by nature",
            Description = "Category applies to products and services which do not attract VAT at 16% by nature.",
            Group = TaxGroup.VAT, Rate = 0
        },
        new TaxType
        {
            Code = "D", Id = 6, Name = "Exempt",
            Description =
                "No tax charge The category applies to specific class of products and services exempted from VAT",
            Group = TaxGroup.VAT, Rate = 0
        },
        new TaxType
        {
            Code = "E", Id = 7, Name = "Disbursement",
            Description =
                "Applies to expenses incurred by suppliers on behalf of customers while supplying a product or service.",
            Group = TaxGroup.VAT
        },
        new TaxType
        {
            Code = "RVAT", Id = 8, Name = "Reverse VAT",
            Description =
                "Applies to transactions involving imported services. (For both Appointed Tax Agent and Self-invoicing).",
            Group = TaxGroup.VAT, Rate = 16
        },
        new TaxType
        {
            Code = "IPL1", Id = 9, Name = "Insurance Premium Levy",
            Description =
                "Applies to the hospitality industry and computed on a total net transaction amount. Applies to all classes of insurance except re-insurance.",
            Group = TaxGroup.IPL, Rate = 5
        },
        new TaxType
        {
            Code = "IPL2", Id = 10, Name = "Re-Insurance", Description = "Applies only to re-insurance.",
            Group = TaxGroup.IPL
        },
        new TaxType
        {
            Code = "TL", Id = 11, Name = "Tourism Levy",
            Description = "Applies to accommodation and conference facilities.", Group = TaxGroup.TL,
            Rate = (decimal)1.5
        },
        new TaxType
        {
            Code = "F", Id = 15, Name = "Service Charge 10%", Description = "Applies to services rendered",
            Group = TaxGroup.VAT, Rate = 10
        },
        new TaxType
        {
            Code = "ECM", Id = 12, Name = "Excise on Coal", Description = "The tax rate is charged per ton",
            Group = TaxGroup.EXCISE, Rate = 5
        },
        new TaxType
        {
            Code = "EXEEG", Id = 13, Name = "Excise Electricity", Description = "The tax rate is charged per 100kwh",
            Group = TaxGroup.EXCISE, Rate = 3
        },
        new TaxType
        {
            Code = "TOT", Id = 14, Name = "Turnover Tax",
            Description =
                "No tax charge Direct tax – tax amount does not reflect on an invoice. For purposes of accounting for turnover relating to taxpayers not",
            Group = TaxGroup.TOT, Rate = 0
        }
    };
}