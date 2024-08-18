using ZambeziDigital.Base.Accounting.Initializers;

namespace ZambeziDigital.Base.Accounting.Contracts;

public interface IInitializesSale
{
    public SaleInitializer InitializeSale();
}

public interface IInitializesSaleItem
{
    public SaleItemInitializer InitializeSaleItem(string? taxCode = null);
}
