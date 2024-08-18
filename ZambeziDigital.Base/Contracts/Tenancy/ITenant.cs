using ZambeziDigital.Base.Contracts.Base;

namespace ZambeziDigital.Base.Contracts.Tenancy;

public interface ITenant : IBaseModel<int> 
    // where TAddress : IAddress
{
    public string? ConnectionString { get; set; }
    public string? Logo { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int? SubscriptionId { get; set; }

    //Navigation Properties
    // public TSubscription? Subscription { get; set; }
    // public List<TUser> Users { get; set; }
}

// public interface ITenant : IBaseModel<int>, IHasKey<int>
//     // where TAddress : IAddress
// {
//     public string? ConnectionString { get; set; }
//     public string? Logo { get; set; }
//     public string? Phone { get; set; }
//     public string? Email { get; set; }
//     public string? Website { get; set; }
//     public int? SubscriptionId { get; set; }
//
//     //Navigation Properties
//     // public TSubscription? Subscription { get; set; }
//     // public List<TUser> Users { get; set; }
// }




