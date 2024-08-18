namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<SaleReceiptType> SaleReceiptTypes = new List<SaleReceiptType>
    {
        new SaleReceiptType {Code = "S", Id = 1, Name = "Sale", Description = "Sale"},
        new SaleReceiptType {Code = "R", Id = 2, Name = "Reversal after Sale", Description = "Refund after Sale Credit Note"},
        new SaleReceiptType {Code = "D", Id = 3, Name = "Adjustment upwards after Sale", Description = "Adjustment upwards after Sale Debit Note"} //TODO: Not implemented
    };

}