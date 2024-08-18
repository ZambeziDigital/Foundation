namespace ZambeziDigital.Base.Accounting.Initializers;

public class SaleItemInitializer
{
    public required int ItemSequenceNumber { get; set; }
    public required string ItemCode { get; set; }
    public required string ItemClassificationCode { get; set; }
    public required string ItemName { get; set; }
    public required string PackageUnitCode { get; set; }
    public required string QuantityUnitCode { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }
    public required string TaxCode { get; set; }
    // public required TaxType TaxType { get; set; }
}