namespace ZambeziDigital.Base.Accounting.Responses;

public class PurchaseResponse
{
    public string resultCd { get; set; } = "";
    public string resultMsg { get; set; }
    public string resultDt { get; set; }
    public PurchaseResponseData data { get; set; }
    public bool IsSuccess => resultCd == "000";
}

    public class PurchaseResponseData
    {
        // public object saleList { get; set; }
        public List<SaleList>? saleList { get; set; }
    }

    public class ZRAPurchaseItem
    {
        public int? itemSeq { get; set; }
        public string? itemCd { get; set; }
        public string? itemClsCd { get; set; }
        public string? itemNm { get; set; }
        public string? pkgUnitCd { get; set; }
        public int? pkg { get; set; }
        public string? qtyUnitCd { get; set; }
        public int? qty { get; set; }
        public decimal? prc { get; set; }
        public double? splyAmt { get; set; }
        public double? dcRt { get; set; }
        public double? dcAmt { get; set; }
        public string? vatCatCd { get; set; }
        public string? iplCatCd { get; set; }
        public double? exciseTaxblAmt { get; set; }
        public double? iplTaxblAmt { get; set; }
        public double? tlTaxblAmt { get; set; }
        public double? taxblAmt { get; set; }
        public double? vatAmt { get; set; }
        public double? iplAmt { get; set; }
        public double? tlAmt { get; set; }
        public double? exciseTxAmt { get; set; }
        public double? totAmt { get; set; }
    }

    public class SaleList
    {
        public string? spplrTpin { get; set; }
        public string? spplrNm { get; set; }
        public string? spplrBhfId { get; set; }
        public int spplrInvcNo { get; set; }
        public string? rcptTyCd { get; set; }
        public string? pmtTyCd { get; set; }
        public string? cfmDt { get; set; }
        public string? salesDt { get; set; }
        public object? stockRlsDt { get; set; }
        public int totItemCnt { get; set; }
        public double totTaxblAmt { get; set; }
        public double totTaxAmt { get; set; }
        public double totAmt { get; set; }
        public string remark { get; set; }
        // public object? itemList { get; set; }
        public List<ZRAPurchaseItem>? itemList { get; set; }
    }
