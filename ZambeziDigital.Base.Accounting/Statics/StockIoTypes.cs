namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<StockIOType> StockIoTypes = new List<StockIOType>
    {
        new StockIOType {Code = "01", Id = 1, Name = "Import", Description = "Incoming-Import"},
        new StockIOType {Code = "02", Id = 2, Name = "Purchase", Description = "Incoming- Purchase"},
        new StockIOType {Code = "03", Id = 3, Name = "Return", Description = "Incoming- Return"},
        new StockIOType {Code = "04", Id = 4, Name = "Stock Movement", Description = "Incoming- Stock Movement"},
        new StockIOType {Code = "05", Id = 5, Name = "Processing", Description = "Incoming- Processing"},
        new StockIOType {Code = "06", Id = 6, Name = "Adjustment", Description = "Incoming- Adjustment"},
        new StockIOType {Code = "11", Id = 7, Name = "Sale", Description = "Outgoing- Sale"},
        new StockIOType {Code = "12", Id = 8, Name = "Return", Description = "Outgoing- Return"},
        new StockIOType {Code = "13", Id = 9, Name = "Stock Movement", Description = "Outgoing- Stock Movement"},
        new StockIOType {Code = "14", Id = 10, Name = "Processing", Description = "Outgoing- Processing"},
        new StockIOType {Code = "15", Id = 11, Name = "Discarding", Description = "Outgoing- Discarding"},
        new StockIOType {Code = "16", Id = 12, Name = "Adjustment", Description = "Outgoing- Adjustment"}
    };
}