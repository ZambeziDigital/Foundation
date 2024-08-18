using ZambeziDigital.Base.Accounting.Models;

namespace ZambeziDigital.Base.Accounting.Requests.SalesRequest;

public partial class SaleRequest
{
    public decimal TaxAmtA=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "A").Sum(x=>x.VatAmt);
    public decimal TaxAmtB=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "B").Sum(x=>x.VatAmt);
    public decimal TaxAmtC1=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C1").Sum(x=>x.VatAmt);
    public decimal TaxAmtC2=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C2").Sum(x=>x.VatAmt);
    public decimal TaxAmtC3=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C3").Sum(x=>x.VatAmt);
    public decimal TaxAmtD=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "D").Sum(x=>x.VatAmt);
    public decimal TaxAmtRvat=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "RVAT").Sum(x=>x.VatAmt);
    public decimal TaxAmtE=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "E").Sum(x=>x.VatAmt);
    public decimal TaxAmtF=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "F").Sum(x=>x.VatAmt);
    public decimal TaxAmtIpl1=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "IPL1").Sum(x=>x.VatAmt);
    public decimal TaxAmtIpl2=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "IPL2").Sum(x=>x.VatAmt);
    public decimal TaxAmtTl=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "TL").Sum(x=>x.VatAmt);
    public decimal TaxAmtEcm=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "EMC").Sum(x=>x.VatAmt);
    public decimal TaxAmtExeeg=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "EXEEG").Sum(x=>x.VatAmt);
    public decimal TaxAmtTot=> Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "TOT").Sum(x=>x.VatAmt);
}