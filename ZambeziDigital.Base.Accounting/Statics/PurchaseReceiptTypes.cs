namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<PurchaseReceiptType> PurchaseReceiptTypes = new List<PurchaseReceiptType>
    {
        new PurchaseReceiptType {Code = "P", Id = 1, Name = "Purchase", Description = "Purchase"},
        new PurchaseReceiptType {Code = "R", Id = 2, Name = "Refund after Purchase", Description = "Refund after Purchase"}
    };
}