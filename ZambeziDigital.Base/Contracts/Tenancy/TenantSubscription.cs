using ZambeziDigital.Base.Contracts.Base;

namespace ZambeziDigital.Base.Contracts.Tenancy
{
    public interface ITenantSubscription<TTenant, TSubecription> : IBaseModel<int>
    {
        public int SubscriptionId { get; set; }
        public int TenantId { get; set; }
        public int PaymentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //Navigation properties
        public TSubecription? Subscription { get; set; }
        public TTenant? Tenant { get; set; }
    }
}