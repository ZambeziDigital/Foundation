using Shared.Models.ZRA.Models;
using ZambeziDigital.Base.Accounting.Models;

namespace ZambeziDigital.Base.Accounting.Initializers;

public class SaleInitializer
{
    // public string TaxPayerNumber { get; set; }
    // public string BranchCode { get; set; }
    public required string CertifiedInvoicingSystemInvoiceNumber { get; set; }

    /// <summary>
    /// Date time when invoice was issued
    /// </summary>
    public required string ConfirmationDate { get; set; }
    public required string SalesDate { get; set; }
    public required List<SaleItem> ItemList { get; set; }
    public int OriginalInvoiceNumber { get; set; }

    private string? _originalSalesDataControllerId;
    public string? OriginalSalesDataControllerId
    {
        get => _originalSalesDataControllerId;
        set
        {
            if (OriginalInvoiceNumber != null && OriginalInvoiceNumber != 0)
            {
                if (value == null || value is "0" || value is "00" || value is "000" || value is "")
                {
                    throw new Exception("OriginalSalesDataControllerId is required when OriginalInvoiceNumber is provided");
                }
            }
            else
            {
                if (_originalSalesDataControllerId != null)
                {
                    throw new Exception("OriginalSalesDataControllerId must be null when OriginalInvoiceNumber is not provided");
                }
            }
            _originalSalesDataControllerId = value;
        }
    }
    public string CurrencyCode { get; set; } = "ZMW";
    public string Remark { get; set; } = string.Empty;
    public string? CustomerTaxPayerNumber { get; set; }
    public string? CustomerName { get; set; }
    public string? LocalPurchaseOrderNumber { get; set; }
    public string? DestinationCountryCode { get; set; }
    //public string? OriginalSDCID { get; internal set; }
}