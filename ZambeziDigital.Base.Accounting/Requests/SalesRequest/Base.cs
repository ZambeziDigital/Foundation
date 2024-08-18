global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using Shared.Models.ZRA.Models;
global using ZambeziDigital.Base.Accounting.Statics;

namespace ZambeziDigital.Base.Accounting.Requests.SalesRequest;

public partial class SaleRequest
{
    private string tpin;
    /// <summary>
    /// Gets or sets the Tax Payer Identification Number (TPIN).
    /// </summary>
    /// <value>
    /// The TPIN as a string.
    /// </value>
    /// <exception cref="System.Exception">Thrown when the provided TPIN is not a 10-digit number.</exception>
    /// <remarks>
    /// The TPIN must be a 10-digit number. If the provided TPIN is not a 10-digit number, an exception is thrown.
    /// </remarks>
    [MaxLength(10)]
    public required string Tpin
    {
        get => tpin;
        set
        {
            //check if incoming value is null, and if the incoming value is all digital and has strictly 10 characters
            if (value == null || value.All(char.IsDigit) && value.Length == 10)
            {
                tpin = value;
            }
            else
            {
                throw new Exception("Tax payer number is invalid, number must be 10 digits");
            }
        }
    }
    [MaxLength(3)]
    private string bhfId;
    /// <summary>
    /// Gets or sets the Branch ID.
    /// </summary>
    /// <value>
    /// The Branch ID as a string.
    /// </value>
    /// <exception cref="System.Exception">Thrown when the provided Branch ID is not a 3-digit number.</exception>
    /// <remarks>
    /// The Branch ID must be a 3-digit number. If the provided Branch ID is not a 3-digit number, an exception is thrown.
    /// </remarks>
    public required string BhfId
    {
        get => bhfId;
        set
        {
            //check if the incoming value is all digits and must be 3 characters
            if (value.All(char.IsDigit) && value.Length == 3)
            {
                bhfId = value;
            }
            else
            {
                throw new Exception("Branch ID must be 3 digits");
            }
        }
    }

    private int orgInvcNo;

    public int OrgInvcNo
    {
        get
        {
            //if()   
            return orgInvcNo;
        }
        set
        {
            //TODO: make sure that this a credit not and the original invoice number is provided, and Validate
            orgInvcNo = value;
        }
    }

    [MaxLength(50)] private string cisInvcNo;

    public string CisInvcNo
    {
        get => cisInvcNo;
        set
        {
            //make sure that the incoming value is not null and is less than or equal 50 characters.
            //also makes sure that if this an invoice it should start with "INV"
            //and if it is a credit note it should start with "CRN"
            //and if it is a debit note it should start with "DBN"
            //and it an LPO it should start with "LPO" and if an export invoice it should start with "EXP"
            if (value.Length > 50) throw new Exception("Invoice number must be less than or equal to 50 characters");
            if (value == null) throw new Exception("Invoice number cannot be null");
            if (value.StartsWith("INV") || value.StartsWith("CRN") || value.StartsWith("DBN") || value.StartsWith("LPO") || value.StartsWith("EXP"))
            {
                //TODO: check if the invoice is for what has been provided
                cisInvcNo = value;

            }
            else
            {
                throw new Exception("Invoice number must start with INV, CRN, DBN, LPO or EXP");
            }

        }
    }

    private string? custTpin;

    public string? CustTpin
    {
        get => custTpin;
        set
        {
            //null value is acceptable, and if the incoming value is all digital and has strictly 10 characters
            if (value == null || value.All(char.IsDigit) && value.Length == 10)
            {
                custTpin = value;
            }
            else
            {
                throw new Exception("Customer TPIN is invalid, number must be 10 digits");
            }

        }
    }
    [MaxLength(60)]
    private string? custNm;
    public string? CustNm
    {
        get => custNm;
        set
        {
            //make sure it has no special characters and is less than or equal to 60 characters
            // if (value == null) throw new Exception("Customer name cannot be null");
            if (value.Length > 60) throw new Exception("Customer name must be less than or equal to 60 characters");
            custNm = value;

            // if (value.All(char.IsLetterOrDigit))
            // {
            //     custNm = value;
            // }
            // else
            // {
            //     throw new Exception("Customer name cannot contain special characters");
            // }
        }
    }

    public string SalesTyCd => Codes.TransactionTypes.First(sr => sr.Id == 2).Code;
    // {
    //     get => salesTyCd;
    //     set
    //     {
    //         if (Codes.TransactionTypes.Select(sr=>sr.Code).Contains(value))
    //         {
    //             salesTyCd = value;
    //         }
    //         else if(Codes.TransactionTypes.Select(sr=>sr.Name).Contains(value))
    //         {
    //             salesTyCd = Codes.TransactionTypes.FirstOrDefault(sr=>sr.Name == value).Code;
    //         }
    //         else
    //         {
    //             throw new Exception("Invalid sales type code");
    //         }
    //         //TODO: check if the sales type code is valid and matches the sales type code in the ZRA
    //         
    //     }
    // }


    public string RcptTyCd => (orgInvcNo == null || orgInvcNo == 0) ?
        Codes.SaleReceiptTypes.First(sr => sr.Id == 1).Code : //This is a sale
        Codes.SaleReceiptTypes.First(sr => sr.Id == 2).Code; //This is a reversal
    //TODO : do number three


    private string pmtTyCd = Codes.PaymentMethods.First(sr => sr.Id == 1).Code;

    public string PmtTyCd
    {
        get => pmtTyCd;
        // set
        // {
        //     if (Codes.PaymentMethods.Select(sr=>sr.Code).Contains(value))
        //     {
        //         pmtTyCd = value;
        //     }
        //     else if(Codes.PaymentMethods.Select(sr=>sr.Name).Contains(value))
        //     {
        //         pmtTyCd = Codes.PaymentMethods.FirstOrDefault(sr=>sr.Name == value).Code;
        //     }
        //     else
        //     {
        //         throw new Exception("Invalid Payment Method code");
        //     }
        // }
    }

    private string salesSttsCd = Codes.TransactionProgresses.First(sr => sr.Id == 1).Code;

    public string SalesSttsCd
    {
        get => salesSttsCd;
        // set
        // {
        //     if (Codes.TransactionProgresses.Select(sr=>sr.Code).Contains(value))
        //     {
        //         salesSttsCd = value;
        //     }
        //     else if(Codes.TransactionProgresses.Select(sr=>sr.Name).Contains(value))
        //     {
        //         salesSttsCd = Codes.TransactionProgresses.FirstOrDefault(sr=>sr.Name == value).Code;
        //     }
        //     else
        //     {
        //         throw new Exception("Invalid Payment Method code");
        //     }
        // }
    }

    private string cfmDt;

    public string CfmDt
    {
        get => cfmDt;
        set
        {
            // check is value is already in yyyyMMddhhmmss format
            if (value.Length == 14 && value.All(char.IsDigit))
            {
                cfmDt = value;
            }
            //try parse the datetime string provided into yyyyMMddhhmmss format
            if (DateTime.TryParseExact(value, "yyyyMMddhhmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                cfmDt = value;
            }
            else
            {
                throw new Exception("Invalid date format");
            }
        }
    }

    private string salesDt;

    public string SalesDt
    {
        get => salesDt;
        set
        {
            if (value.Length == 14 && value.All(char.IsDigit))
            {
                salesDt = value;
            }
            //try parse the datetime string provided into yyyyMMddhhmmss format
            if (DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                salesDt = value;
            }
            else
            {
                throw new Exception("Invalid date format");
            }
        }
    }

}