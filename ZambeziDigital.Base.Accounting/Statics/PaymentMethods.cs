namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    
    public static readonly List<PaymentMethod> PaymentMethods = new List<PaymentMethod>
    {
        new PaymentMethod {Code = "01", Id = 1, Name = "CASH", Description = "CASH"},
        new PaymentMethod {Code = "02", Id = 2, Name = "CREDIT", Description = "CREDIT"},
        new PaymentMethod {Code = "03", Id = 3, Name = "CASH/CREDIT", Description = "CASH/CREDIT"},
        new PaymentMethod {Code = "04", Id = 4, Name = "BANK CHECK", Description = "BANK CHECK PAYMENT"},
        new PaymentMethod {Code = "05", Id = 5, Name = "DEBIT&CREDIT CARD", Description = "PAYMENT USING CARD"},
        new PaymentMethod {Code = "06", Id = 6, Name = "MOBILE MONEY", Description = "ANY TRANSACTION USING MOBILE MONEY SYSTEM"},
        new PaymentMethod {Code = "07", Id = 7, Name = "Bank transfer", Description = "Bank transfer"},
        new PaymentMethod {Code = "08", Id = 8, Name = "OTHER", Description = "OTHER MEANS OF PAYMENT"}
    };
}