global using System.Text.Json.Serialization;

namespace ZambeziDigital.Base.Accounting.Requests;

    public class PurchaseAcceptItemRequest
    {
        [JsonPropertyName("itemSeq")]
        public int itemSeq { get; set; }
        [JsonPropertyName("itemCd")]
        public string ItemCode { get; set; }
        private string itemClassificationCode;
        [JsonPropertyName("itemClsCd")]
        public string ItemClassificationCode { 
            get { return itemClassificationCode ?? throw new Exception("Classification can not be obtained because it is null"); }
            set
            {
                if (string.IsNullOrEmpty(value)) itemClassificationCode = value;
                else
                {
                    // if (Codes..Any(x=>x.Code==value))
                        itemClassificationCode = value;
                    // else
                        // throw new Exception("Invalid Item Classification code");
                }
            }}
        [JsonPropertyName("itemNm")]
        public string ItemName { get; set; }

        [JsonPropertyName("bcd")] public object Barcode => null;
        private string? pkgUnitCd;
        [JsonPropertyName("pkgUnitCd")]
        public string PackagingUnitCode {
            get
            {
                return pkgUnitCd ?? throw new Exception("Packaging Unit Code can not be null");  
            }
            set
            {
                if (string.IsNullOrEmpty(value)) pkgUnitCd = value;
                else
                {
                    if (Codes.PackagingUnits.Any(x=>x.Code==value))
                        pkgUnitCd = value;
                    else
                        throw new Exception("Invalid Packaging Unit code");
                }
            } 
        }

        private int pkg;
        [JsonPropertyName("pkg")] public int Packages => 0;
        private string qtyUnitCd { get; set; }
        [JsonPropertyName("qtyUnitCd")]
        public string QuantityUnitCode {
            get
            {
                return qtyUnitCd ?? throw new Exception("Quantity Unit Code can not be null");  
            }
            set
            {
                if (string.IsNullOrEmpty(value)) qtyUnitCd = value;
                else
                {
                    if (Codes.UnitsOfMeasures.Any(x=>x.Code==value))
                        qtyUnitCd = value;
                    else
                        throw new Exception("Invalid Quantity Unit code");
                }
            } 
        }
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }

        public decimal prc;

        [JsonPropertyName("prc")]
        public decimal Price
        {
            get
            {
                if(prc <= 0) throw new Exception("Price can not be less than or equal to zero");
                return Math.Round(prc, 4) ;
            }
            set
            {
                if(value <= 0) throw new Exception("Price can not be less than or equal to zero");
                prc = value;
            }
        }

        [JsonPropertyName("splyAmt")]
        public decimal SupplyAmmount => Math.Round(Price * Quantity, 4);

        [JsonPropertyName("dcRt")] public int DiscountRate { get; set; } = 0;
        [JsonPropertyName("dcAmt")]
        public int DiscountAmount { get; set; } = 0;
        [JsonPropertyName("taxblAmt")]
        public decimal TaxableAmount {
            get
            {
                if(IsTaxInclusive)
                    return Math.Round(SupplyAmmount / (1 + (TaxAmount / 100)), 4);
                else
                    return SupplyAmmount;
            } 
        }
            // if the item price is tax inclusive calculate the taxsble amount else return the supply amount
        
        
        private string taxTypeCode = null;
        [JsonPropertyName("taxTyCd")]
        public string TaxTypeCode { get; set; }
        
        
        private string insurancePremiumLevyCategoryCode = null;

        [JsonPropertyName("iplCatCd")]
        public string InsurancePremiumLevyCategoryCode
        {
            get => insurancePremiumLevyCategoryCode;
            set
            {
                if (string.IsNullOrEmpty(value)) insurancePremiumLevyCategoryCode = value;
                else
                {
                    if (Codes.TaxTypes.Any(x=>x.Code==value))
                        insurancePremiumLevyCategoryCode = value;
                    else
                        throw new Exception("Invalid Insurance Premium Levy code");
                }
            }
        }
        
        
        
        
        
        private string?  taxLevyCategotyCode = null;

        [JsonPropertyName("tlCatCd")]
        public string TaxLevyCategotyCode
        {
            get => taxLevyCategotyCode;
            set
            {
                if (string.IsNullOrEmpty(value)) taxLevyCategotyCode = value;
                else
                {
                    if (Codes.TaxTypes.Any(x=>x.Code==value))
                        taxLevyCategotyCode = value;
                    else
                        throw new Exception("Invalid Tax Levy code");
                }
            }
        }
        
        
        
        

        private string? exciseCategoryCode = null;
        [JsonPropertyName("exciseCatCd")]
        public string ExciseCategoryCode { get => exciseCategoryCode;
            set
            {
                if (string.IsNullOrEmpty(value)) exciseCategoryCode = value;
                else
                {
                    if (Codes.TaxTypes.Any(x=>x.Code==value))
                        exciseCategoryCode = value;
                    else
                        throw new Exception("Invalid Excise code");
                }
                  
            }}
        

        private string? valueAddedTaxCategoryCode = null;
        [JsonPropertyName("vatCatCd")]
        public string ValueAddedTaxCategoryCode 
        { 
            get => valueAddedTaxCategoryCode;
            set
            {
                if (string.IsNullOrEmpty(value)) valueAddedTaxCategoryCode = value;
                else
                {
                    if (Codes.TaxTypes.Any(x=>x.Code==value))
                        valueAddedTaxCategoryCode = value;
                    else
                        throw new Exception("Invalid VAT code");
                }
            } 
        }
        
        [JsonPropertyName("iplTaxblAmt")]
        public decimal InsurancePremiumLevyTaxableAmount => string.IsNullOrEmpty(InsurancePremiumLevyCategoryCode) ? 0 : TotalAmount;
        [JsonPropertyName("tlTaxblAmt")]
        public decimal TaxLevyTaxableAmount => string.IsNullOrEmpty(TaxLevyCategotyCode) ? 0 : TotalAmount;
        [JsonPropertyName("exciseTaxblAmt")]
        public decimal ExciseTaxableAmount => string.IsNullOrEmpty(ExciseCategoryCode)  ? 0 : TotalAmount;
        
        
        [JsonPropertyName("taxAmt")]
        public decimal TaxAmount  => Math.Round((TaxableAmount * Codes.TaxTypes.FirstOrDefault(x=>x.Code==TaxTypeCode)?.Rate ?? 0), 4);
        
        
        [JsonPropertyName("iplAmt")]
        public decimal InsurancePremiumLevyAmount => Math.Round((InsurancePremiumLevyTaxableAmount * Codes.TaxTypes.FirstOrDefault(x=>x.Code==InsurancePremiumLevyCategoryCode)?.Rate ?? 0), 4);
        
        
        [JsonPropertyName("tlAmt")]
        public decimal TaxLevyAmount =>  Math.Round((TaxAmount * Codes.TaxTypes.FirstOrDefault(x=>x.Code==TaxLevyCategotyCode)?.Rate ?? 0), 4);
        
        
        [JsonPropertyName("exciseTxAmt")]
        public decimal ExciseTaxAmount => Math.Round((ExciseTaxableAmount * Codes.TaxTypes.FirstOrDefault(x=>x.Code==ExciseCategoryCode)?.Rate ?? 0), 4);
        
        
        [JsonPropertyName("totAmt")]
        public decimal TotalAmount => Math.Round((Quantity * Price), 4) ; 
        // public int totAmt { get; set; }
        
        public bool IsTaxInclusive { get; set; } = false;

    }
