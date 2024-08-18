namespace ZambeziDigital.Base.Accounting.Requests.SalesRequest;

public partial class SaleRequest
{
    public decimal TotAmt  => TotTaxblAmt + TotTaxAmt;

    public decimal TotTaxAmt => TaxAmtA + TaxAmtB + TaxAmtC1 + TaxAmtC2 + TaxAmtC3 + TaxAmtD + TaxAmtE + TaxAmtF +
                               TaxAmtRvat + TaxAmtIpl1 + TaxAmtIpl2 + TaxAmtTl + TaxAmtEcm + TaxAmtExeeg + TaxAmtTot;
    public decimal TotTaxblAmt  => TaxblAmtA + TaxblAmtB + TaxblAmtC1 + TaxblAmtC2 + TaxblAmtC3 + TaxblAmtD + TaxblAmtE + TaxblAmtF +
                            TaxblAmtRvat + TaxblAmtIpl1 + TaxblAmtIpl2 + TaxblAmtTl + TaxblAmtEcm + TaxblAmtExeeg + TaxblAmtTot;

    // private string prchrAcptcYn;

    public string PrchrAcptcYn => "N";
    // {
    //     get => prchrAcptcYn;
    //     set
    //     {
    //         //only set is The value is Y or N
    //         if (value == "Y" || value == "N")
    //         {
    //             prchrAcptcYn = value;
    //         }
    //         else if (value == null)
    //         {
    //             prchrAcptcYn = "Y";
    //         }
    //         else
    //         {
    //             throw new Exception("Invalid value for PrchrAcptcYn(Purchase Accepted, Must be Y or N)");
    //         }
    //     }
    // }

    private string? remark;
    public string? Remark { get=>remark; set=> remark=value; }
    public string RegrId => "admin"; //todo: get from session
    public string RegrNm => "admin"; //todo: get from session
    public string ModrId => "admin"; //todo: get from session
    public string ModrNm => "admin"; //todo: get from session
    public string SaleCtyCd => "1";
    private string lpoNumber;
    public string? LpoNumber {
        get
        {
            if (TaxAmtC2 > 0 && string.IsNullOrEmpty(lpoNumber))
                throw new Exception("LPO number can not be null of there is an item with C2 tax code");
            else return lpoNumber;

        }
        set
        {
            if (value == null)
            {
                lpoNumber = value;
            }
            else if (value.Length ==10)
            {
                lpoNumber = value;
            }
            else
            {
                throw new Exception("Local Purchase Order Number can be null or should be a valid LPO number");    
            }
        } 
    }
    private string currencyTyCd;

    public string CurrencyTyCd
    {
        get => currencyTyCd;
        set
        {
            if (Codes.Currencies.Select(sr=>sr.Code).Contains(value))
            {
                currencyTyCd = value;
            }
            else if(Codes.Currencies.Select(sr=>sr.Name).Contains(value))
            {
                currencyTyCd = Codes.Currencies.FirstOrDefault(sr=>sr.Name == value).Code;
            }
            else
            {
                throw new Exception("Invalid Currency code");
            }
        }
    }

    public string ExchangeRt => currencyTyCd == "ZMW" ? "1" :  throw new Exception("Exchange Rate not Impleimented"); //todo: get from session
    private string? destnCountryCd;

    public string DestnCountryCd
    {
        get
        {
            if(TaxAmtC1 > 0 && string.IsNullOrEmpty(destnCountryCd))
                throw new Exception("Destination Country Code can not be null if there is an item with C1 tax code");
            else return destnCountryCd ?? "";
        }
        set
        {
            if (value == null)
            {
                destnCountryCd = value;
            }
            else if(Codes.Countries.Select(sr=>sr.Code).Contains(value))
            {
                destnCountryCd = value;
            }
            else
            {
                throw new Exception("Destination Country Code can be null or should be a valid 2 letter country code");
            }
        }
    }

    private string dbtRsnCd;
    public string DbtRsnCd => "";
    // {
    //     get => "";//dbtRsnCd;
    //     set
    //     {
    //         throw new NotImplementedException();
    //         if (ZRAStatics.Currencies.Select(sr=>sr.Code).Contains(value))
    //         {
    //             currencyTyCd = value;
    //         }
    //         else if(ZRAStatics.Currencies.Select(sr=>sr.Name).Contains(value))
    //         {
    //             currencyTyCd = ZRAStatics.Currencies.FirstOrDefault(sr=>sr.Name == value).Code;
    //         }
    //         else
    //         {
    //             throw new Exception("Invalid Currency code");
    //         }
    //     }
    // }

    public string InvcAdjustReason => "";
}