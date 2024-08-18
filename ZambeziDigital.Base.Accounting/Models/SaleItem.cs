namespace ZambeziDigital.Base.Accounting.Models;

public class SaleItem
{
    public SaleItem(int itemSeq, string itemCd, string itemClsCd, string itemNm, string pkgUnitCd, string qtyUnitCd, decimal qty, decimal prc, string vatCatCd, TaxType taxType)
    {
        ItemSeq = itemSeq;
        ItemCd = itemCd;
        ItemClsCd = itemClsCd;
        ItemNm = itemNm;
        PkgUnitCd = pkgUnitCd;
        QtyUnitCd = qtyUnitCd;
        Qty = qty;
        Prc = prc;
        VatCatCd = vatCatCd;
        // TaxType = taxType;
    }

    public SaleItem(SaleItemInitializer saleItemInitializer)
    {
        
        ItemSeq = saleItemInitializer.ItemSequenceNumber;
        ItemCd = saleItemInitializer.ItemCode;
        ItemClsCd = saleItemInitializer.ItemClassificationCode;
        ItemNm = saleItemInitializer.ItemName;
        PkgUnitCd = saleItemInitializer.PackageUnitCode;
        QtyUnitCd = saleItemInitializer.QuantityUnitCode;
        Qty = saleItemInitializer.Quantity;
        Prc = saleItemInitializer.Price;
        VatCatCd = saleItemInitializer.TaxCode;
        // TaxType = saleItemInitializer.TaxType;
    }
    
    public SaleItem(){}
    
    private int itemSeq;
    public  int ItemSeq { get => itemSeq; set => itemSeq=value; }
    // public required int ItemSequenceNumber { get => itemSeq; set => itemSeq=value; }

    private string itemCd;
    public  string ItemCd { get=>itemCd; set=> itemCd=value; }
    // public required string ItemCode { get=>itemCd; set=> itemCd=value; }

    private string itemClsCd;
    public  string ItemClsCd { get=>itemClsCd; set=>itemClsCd=value; }
    // public required string ItemClassificationCode { get=>itemClsCd; set=>itemClsCd=value; }

    private string itemNm;
    // public required string ItemName { get=>itemNm; set=>itemNm=value; }
    public  string ItemNm { get=>itemNm; set=>itemNm=value; }
    
    private string Bcd => String.Empty;

    private string pkgUnitCd;
    // public required string PackageUnitCode
    public  string PkgUnitCd
    {
        get=>pkgUnitCd;
        set
        {
            if (Codes.PackagingUnits.Select(sr => sr.Code).Contains(value))
            {
                pkgUnitCd = value;
            }
            else if (Codes.PackagingUnits.Select(sr => sr.Name).Contains(value))
            {
                pkgUnitCd = Codes.PackagingUnits.FirstOrDefault(sr => sr.Name == value).Code;
            }
            else
            {
                throw new Exception("Invalid PackageUnitCode code");
            }
        }
    }

    public decimal Pkg => 0;
    private string qtyUnitCd { get; set; }

    // public required string QuantityUnitCode
    public  string QtyUnitCd
    {
        get => qtyUnitCd;
        set
        {
            if (Codes.UnitsOfMeasures.Select(sr => sr.Code).Contains(value))
            {
                qtyUnitCd = value;
            }
            else if (Codes.UnitsOfMeasures.Select(sr => sr.Name).Contains(value))
            {
                qtyUnitCd = Codes.UnitsOfMeasures.FirstOrDefault(sr => sr.Name == value).Code;
            }
            else
            {
                throw new Exception("Invalid qtyUnitCd code");
            }
        }
    }

    private decimal qty { get; set; }

    public decimal Qty
    {
        get => qty;
        set
        {
            qty=value;
        }
    }

    private decimal prc { get; set; }

    public decimal Prc
    {
        get=> Math.Round(prc, 4); 
        set=>prc=value;
    }
    // public required decimal Price { get=>prc; set=>prc=value; }
    public decimal SplyAmt => TotAmt;// ShadowTotal;

    public decimal DcRt => 0; //todo: implement discounts
    public decimal DcAmt => 0;//todo: implement discounts
    public string IsrccCd => ""; //todo: implement inssuarance
    public string IsrccNm => "";//todo: implement inssuarance
    public decimal IsrcRt => 0;
    public decimal IsrcAmt => 0;

    /// <summary>
    /// This is the VAT category code of a registered item. Where not applicable, pass null
    /// </summary>
    private string vatCatCd;

    public string VatCatCd
    {
        get => vatCatCd;
        set
        {

            if (Codes.TaxTypes.Select(sr => sr.Code).Contains(value))
            {
                vatCatCd = value;
            }
            else if (Codes.TaxTypes.Select(sr => sr.Name).Contains(value))
            {
                vatCatCd = Codes.TaxTypes.FirstOrDefault(sr => sr.Name == value).Code;
            }
            else
            {
                throw new Exception("Invalid Currency code");
            }
        }
    }

    public string? ExciseTxCatCd => null;
    public string? TlCatCd => null;
    public string? IplCatCd => null;

    public decimal VatTaxblAmt => (Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd))?.Rate != null ?  Math.Round(((prc*qty) / (decimal)(1 + ((Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd)).Rate / 100))), 4) : 0;

    public decimal VatAmt
    {
        get
        {
            if ((Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd)) == null)
            {
                return 0;
            }
            return Math.Round((VatTaxblAmt * ((Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd)).Rate / 100)), 4);
        }
    }

    public int ExciseTaxblAmt => 0;
    public decimal TlTaxblAmt => 0;
    public decimal IplTaxblAmt => 0;
    public decimal IplAmt => 0;
    public decimal TlAmt => 0;
    public int ExciseTxAmt => 0;
    public decimal TotAmt => Math.Round((qty * prc), 4) ; 
    
    
    // public  TaxType TaxType  => Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd);  
    // public required TaxType TaxType { get; set; }
    // public decimal ShadowTotal  => (Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd))?.Rate != null ?  Math.Round(((prc*qty) / (decimal)(1 + ((Codes.TaxTypes.FirstOrDefault(sr => sr.Code == vatCatCd) ?? throw new Exception("TaxType not found, vatCatCd = " + vatCatCd)).Rate / 100))), 4) : 0; 
}
