namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<TransactionProgressStatus> TransactionProgresses = new List<TransactionProgressStatus>
    {
        new TransactionProgressStatus {Code = "02", Id = 1, Name = "Approved", Description = "Approved"},
        new TransactionProgressStatus {Code = "05", Id = 2, Name = "Refunded", Description = "Refunded"},
        new TransactionProgressStatus {Code = "06", Id = 3, Name = "Transferred", Description = "Transferred"}
    };

}