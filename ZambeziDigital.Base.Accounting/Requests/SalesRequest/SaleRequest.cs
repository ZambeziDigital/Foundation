global using Newtonsoft.Json;
global using ZambeziDigital.Base.Accounting.Initializers;
using ZambeziDigital.Base.Accounting.Models;

namespace ZambeziDigital.Base.Accounting.Requests.SalesRequest;

public partial class SaleRequest
{
    // public SaleRequest(string tpin, string bhfId, int orgInvcNo, string cisInvcNo, string custTpin, string custNm, string salesTyCd, string rcptTyCd, string pmtTyCd, string salesSttsCd, string cfmDt, string salesDt, string prchrAcptcYn, string remark, string currencyTyCd, List<SaleItem> itemList)
    // {
    //     TaxPayerNumber = tpin;
    //     BranchCode = bhfId;
    //     OriginalInvoiceNumber = orgInvcNo;
    //     CertifiedInvoicingSystemInvoiceNumber = cisInvcNo;
    //     CustomerTaxPayerNumber = custTpin;
    //     CustomerName = custNm;
    //     SalesTyCd = salesTyCd;
    //     RcptTyCd = rcptTyCd;
    // pmtTyCd = pmtTyCd;
    //     SalesSttsCd = salesSttsCd;
    //     ConfirmationDate = cfmDt;
    //     SalesDate = salesDt;
    //     PrchrAcptcYn = prchrAcptcYn;
    //     Remark = remark;
    //     CurrencyCode = currencyTyCd;
    //     ItemList = itemList;
    // }

    public SaleRequest(SaleInitializer saleInitializer)
    {
        // Tpin = saleInitializer.TaxPayerNumber;
        // BhfId = saleInitializer.BranchCode;
        OrgInvcNo = saleInitializer.OriginalInvoiceNumber;
        CisInvcNo = saleInitializer.CertifiedInvoicingSystemInvoiceNumber;
        if (saleInitializer.CustomerTaxPayerNumber != null)
            CustTpin = saleInitializer.CustomerTaxPayerNumber;
        if (saleInitializer.CustomerName != null)
            CustNm = saleInitializer.CustomerName;
        CfmDt = saleInitializer.ConfirmationDate;
        SalesDt = saleInitializer.SalesDate;
        Remark = saleInitializer.Remark;
        CurrencyTyCd = saleInitializer.CurrencyCode;
        ItemList = saleInitializer.ItemList;
        LpoNumber = saleInitializer.LocalPurchaseOrderNumber;
        DestnCountryCd = saleInitializer.DestinationCountryCode;
        OrgSdcId = saleInitializer.OriginalSalesDataControllerId;
    }
    // public SaleRequest()
    // {
    // }

    private object? stockRlsDt;
    public object? StockRlsDt
    {
        get => null;
    }

    private object? cnclReqDt;
    public object? CnclReqDt => null;

    private object? cnclDt;
    public object? CnclDt => null;

    private object? rfdDt;
    public object? RfdDt => null;


    private string? rfdRsnCd;
    public string? RfdRsnCd => OrgInvcNo == null || OrgInvcNo == 0 ? null : "06";


    // private int totItemCnt { get; set; }
    public int TotItemCnt => ItemList.Count;





    public List<SaleItem> ItemList { get; set; } = new();
    // public required List<SaleItem> ItemList { get; set; } = new();

    [JsonProperty("orgSdcId ")]
    private string orgSdcId { get; set; }

    [JsonProperty("orgSdcId ")]
    public string OrgSdcId
    {
        get
        {
            if (orgInvcNo != 0 && orgSdcId == null)
                throw new Exception("Please make sure that the original SDC ID is proveded");
            return orgSdcId;
        }
        set
        {
            orgSdcId = value;
            //if(orgInvcNo != null && orgInvcNo != 0)
            //{
            //    if(orgSdcId == null || orgSdcId is "0"|| orgSdcId is "00"|| orgSdcId is "000"|| orgSdcId is "")
            //    {
            //        throw new Exception("OriginalSalesDataControllerId is required when OriginalInvoiceNumber is provided");
            //    }
            //}
            //else
            //{
            //    if(orgSdcId != null)
            //    {
            //        throw new Exception("OriginalSalesDataControllerId must be null when OriginalInvoiceNumber is not provided");
            //    }
            //}
            //orgSdcId = value;
        }
    }
}