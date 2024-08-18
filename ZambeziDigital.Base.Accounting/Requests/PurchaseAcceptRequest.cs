namespace ZambeziDigital.Base.Accounting.Requests;
    public class PurchaseAcceptRequest
    {
        [JsonPropertyName("tpin")]
        public string TPIN { get; set; }
        [JsonPropertyName("bhfId")]
        public string BranchCode { get; set; }
        [JsonPropertyName("invcNo")]
        public string InvoiceNumber { get; set; }
        [JsonPropertyName("orgInvcNo")]
        public int OriginalInvoiceNumber { get; set; }
        [JsonPropertyName("spplrTpin")]
        public string SupplierTPIN { get; set; }
        [JsonPropertyName("spplrBhfId")]
        public string SupplierBranchCode { get; set; }
        [JsonPropertyName("spplrNm")]
        public string SupplierName { get; set; }
        [JsonPropertyName("spplrInvcNo")]
        public string SupplierInvoiceNumber { get; set; }
        [JsonPropertyName("regTyCd")]
        public string RegistrationTypeCode { get; set; }
        [JsonPropertyName("pchsTyCd")]
        public string PurchaseTypeCode { get; set; }
        [JsonPropertyName("rcptTyCd")]
        public string ReceiptTypeCode { get; set; }
        [JsonPropertyName("pmtTyCd")]
        public string PaymentTypeCode { get; set; }
        [JsonPropertyName("pchsSttsCd")]
        public string PurchaseStatusCode { get; set; }
        [JsonPropertyName("cfmDt")]
        public string ConfirmedDate   { get; set; }
        [JsonPropertyName("pchsDt")]
        public string PurchaseDate { get; set; }
        [JsonPropertyName("totItemCnt")]
        public int TotalItemCount => itemList.Count;
        [JsonPropertyName("totTaxblAmt")]
        public decimal TotalTaxableAmount => itemList.Sum(x => x.TaxableAmount);
        [JsonPropertyName("totTaxAmt")]
        public decimal totTaxAmt => itemList.Sum(x => x.TaxAmount);
        [JsonPropertyName("totAmt")]
        public decimal TotalAmount => itemList.Sum(x => x.TotalAmount);

        [JsonPropertyName("remark")] public string remark => "";
        [JsonPropertyName("regrNm")] public string regrNm => "Admin";
        [JsonPropertyName("regrId")] public string regrId => "Admin";
        [JsonPropertyName("modrNm")] public string modrNm => "Admin";
        [JsonPropertyName("modrId")] public string modrId => "Admin";
        [JsonPropertyName("itemList")]
        public List<PurchaseAcceptItemRequest> itemList { get; set; }
    }

