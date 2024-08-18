using ZambeziDigital.Base.Accounting.ZRA.Models;

namespace Shared.Models.ZRA.Models;

public class CreditNoteReason : TypeBase;
public class InsurancePremiumLevy : TypeBase;
public class TourismLevy : TypeBase;
public class ExciseTaxRegistrationStatus : TypeBase;
public class IPLRegistrationStatus : TypeBase;
public class TourismLevyRegistrationStatus : TypeBase;
public class ReasonForDebitNote : TypeBase;
public class RentalIncomeStatus : TypeBase;
public class VATType : TypeBase;
public class ExciseDuty : TypeBase;
public class ReasonOfInventoryAdjustment : TypeBase;
public class SmartInvoiceType : TypeBase;
public class Language : TypeBase;
public class CategoryLevel : TypeBase;
public class PurchaseStatus : TypeBase;
public class TransactionType : TypeBase;
public class SaleReceiptType : TypeBase;
// public class ProductType : TypeBase;
public class Country : TypeBase;
public class SaleCategory : TypeBase;
public class SaleStatus : TypeBase;
public class DefaultInformation : TypeBase;
public class ItemType : TypeBase; //ProductType
public class ImportItemStatus : TypeBase;
public class BranchStatus : TypeBase;
public class QuantityUnit : TypeBase; //6.5. Units of Measure & Quantity Unit
public class PaymentMethod : TypeBase;
public class TransactionProgressStatus : TypeBase;
public class RegistrationType : TypeBase;
public class SaleType : TypeBase;
public class PurchaseReceiptType : TypeBase;
public class PackagingUnit : TypeBase;
public class UnitsOfMeasure : TypeBase; //6.5. Units of Measure & Quantity Unit

public class Currency : TypeBase, IHasKey<int>;
public class StockIOType : TypeBase;
public class TaxType : TypeBase
{
    public TaxGroup Group { get; set; }
    /// <summary>
    /// Rate in percentage
    /// </summary>
    public decimal Rate { get; set; } = 0;
}
public enum TaxGroup
{
    VAT,
    EXCISE,
    TL,
    IPL,
    TOT
}

