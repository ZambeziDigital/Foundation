namespace ZambeziDigital.Multitenancy.Models.Shared
{
    public interface ITenantSubscription : IBaseModel<int>
    {
        public int SubscriptionId { get; set; }
        public int TenantId { get; set; }
        public int PaymentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //Navigation properties
        public ISubscription? Subscription { get; set; }
        public ITenant? Tenant { get; set; }
    }
}