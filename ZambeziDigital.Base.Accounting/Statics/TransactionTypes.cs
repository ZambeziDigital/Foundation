namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    
    public static readonly List<TransactionType> TransactionTypes = new List<TransactionType>
    {
        new TransactionType {Code = "C", Id = 1, Name = "Copy", Description = "Sales and purchase type: Copy"},
        new TransactionType {Code = "N", Id = 2, Name = "Normal", Description = "Sales and purchase type: Normal"}
    };


}