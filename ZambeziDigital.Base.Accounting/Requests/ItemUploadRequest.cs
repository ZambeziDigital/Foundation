namespace ZambeziDigital.Base.Accounting.Requests;

public class ItemUploadRequest
{
     public required string tpin { get; set; }
     public required string bhfId { get; set; }
     public string? itemCd { get; set; }
     public string? itemClsCd { get; set; }
     public string? itemTyCd { get; set; }
     public string? itemNm { get; set; }
     public string? itemStdNm { get; set; }
     public string? orgnNatCd { get; set; }
     public string? pkgUnitCd { get; set; }
     public string? qtyUnitCd { get; set; }
     public string? vatCatCd { get; set; }
     public string? iplCatCd { get; set; }
     public string? tlCatCd { get; set; }
     public string? exciseTxCatCd { get; set; }
     public string? btchNo { get; set; } = null; //Not implemented in the UI
     public string? bcd { get; set; } = null;//Not implemented in the UI
     public decimal? dftPrc { get; set; }
     public string? addInfo { get; set; } = null; //Not implemented in the UI
     public int? sftyQty { get; set; }
     public string? isrcAplcbYn { get; set; }
     public string? useYn { get; set; }
     public string? regrNm { get; set; }
     public string? regrId { get; set; }
     public string? modrNm { get; set; }
     public string? modrId { get; set; }

     // public ItemUploadRequest(Item item)
     // {
     //      this.itemCd = item.Code;
     //      this.itemClsCd = item.ClassificationCode;
     //      this.itemTyCd = item.TypeCode;
     //      this.itemNm = item.Name;
     //      this.itemStdNm = item.StandardName;
     //      this.orgnNatCd = item.OriginCountryCode;
     //      this.pkgUnitCd = item.PackagingUnitCode;
     //      this.qtyUnitCd = item.UnitOfMeasureCode;
     //      this.vatCatCd = item.TaxCode;
     //      this.iplCatCd = null; //TODO:item.TaxType.Code;
     //      this.tlCatCd = null; //TODO:item.TaxType.Code;
     //      this.exciseTxCatCd = null; //TODO:item.TaxType.Code;
     //      this.dftPrc = Math.Round( item.SellingPrice, 4);
     //      this.sftyQty = item.SafetyQuantity;
     //      this.isrcAplcbYn = item.CanBeInsured ? "Y" : "N";
     //      this.useYn = "Y";
     //      this.regrNm = "admin";
     //      this.regrId = "admin";
     //      this.modrNm = "admin";
     //      this.modrId = "admin";
     // }
}
