namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<CreditNoteReason> RefundReasons = new()
    {
        new CreditNoteReason { Code = "01", Id = 1, Name = "Missing Quantity", Description = "Missing Quantity" },
        new CreditNoteReason { Code = "02", Id = 2, Name = "Missing Item", Description = "Missing Item" },
        new CreditNoteReason { Code = "03", Id = 3, Name = "Damaged", Description = "Damaged" },
        new CreditNoteReason { Code = "04", Id = 4, Name = "Wasted", Description = "Wasted" },
        new CreditNoteReason
            { Code = "05", Id = 5, Name = "Raw Material Shortage", Description = "Raw Material Shortage" },
        new CreditNoteReason { Code = "06", Id = 6, Name = "Refund", Description = "Refund" },
        new CreditNoteReason { Code = "07", Id = 7, Name = "Wrong Customer TPIN", Description = "Wrong Customer TPIN" },
        new CreditNoteReason { Code = "08", Id = 8, Name = "Wrong Customer name", Description = "Wrong Customer name" },
        new CreditNoteReason { Code = "09", Id = 9, Name = "Wrong Amount/price", Description = "Wrong Amount/price" },
        new CreditNoteReason { Code = "10", Id = 10, Name = "Wrong Quantity", Description = "Wrong Quantity" },

        new CreditNoteReason { Code = "11", Id = 11, Name = "Wrong Item(s)", Description = "Wrong Item(s)" },
        new CreditNoteReason { Code = "12", Id = 12, Name = "Wrong tax type", Description = "Wrong tax type" },
        new CreditNoteReason { Code = "13", Id = 13, Name = "Other reason", Description = "Other reason" }

    };
}