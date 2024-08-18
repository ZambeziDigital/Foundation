using ZambeziDigital.Base.Accounting.Models;

namespace ZambeziDigital.Base.Accounting.Requests.SalesRequest;

public partial class SaleRequest
{

    public decimal TaxblAmtA => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "A").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtB => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "B").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtC1 => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C1").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtC2 => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C2").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtC3 => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "C3").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtD => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "D").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtRvat => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "RVAT").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtE => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "E").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtF => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "F").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtIpl1 => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "IPL1").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtIpl2 => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "IPL2").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtTl => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "TL").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtEcm => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "ECM").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtExeeg => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "EXEEG").Sum(x=>x.VatTaxblAmt);
    public decimal TaxblAmtTot => Enumerable.Where<SaleItem>(ItemList, x=>x.VatCatCd == "TOT").Sum(x=>x.VatTaxblAmt);
    
    
    public decimal TaxRtA => Codes.TaxTypes.First(x=>x.Code == "A").Rate;
    public decimal TaxRtB => Codes.TaxTypes.First(x=>x.Code == "B").Rate;
    public decimal TaxRtC1 => Codes.TaxTypes.First(x=>x.Code == "C1").Rate;
    public decimal TaxRtC2 => Codes.TaxTypes.First(x=>x.Code == "C2").Rate;
    public decimal TaxRtC3 => Codes.TaxTypes.First(x=>x.Code == "C3").Rate;
    public decimal TaxRtD => Codes.TaxTypes.First(x=>x.Code == "D").Rate;
    public decimal TlAmt => 0;
    public decimal TaxRtRvat => Codes.TaxTypes.First(x => x.Code == "RVAT").Rate;
    public decimal TaxRtE => Codes.TaxTypes.First(x=>x.Code=="E").Rate;
    public decimal TaxRtF => Codes.TaxTypes.First(x=>x.Code=="F").Rate;
    public decimal TaxRtIpl1 => Codes.TaxTypes.First(x=>x.Code=="IPL1").Rate;
    public decimal TaxRtIpl2 => Codes.TaxTypes.First(x=>x.Code=="IPL2").Rate;
    public decimal TaxRtTl => Codes.TaxTypes.First(x=>x.Code=="TL").Rate;
    public decimal TaxRtEcm => Codes.TaxTypes.First(x=>x.Code=="ECM").Rate;
    public decimal TaxRtExeeg => Codes.TaxTypes.First(x=>x.Code=="EXEEG").Rate;
    public decimal TaxRtTot => Codes.TaxTypes.First(x=>x.Code=="TOT").Rate;
}