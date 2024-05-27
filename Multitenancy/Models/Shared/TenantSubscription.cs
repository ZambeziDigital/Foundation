namespace ZambeziDigital.Multitenancy.Models.Shared
{
    public class TenantSubscription : BaseModel
    {
        public int SubscriptionId { get; set; }
        public int TenantId { get; set; }
        public int PaymentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //Navigation properties
        public Subscription? Subscription { get; set; }
        public Tenant? Tenant { get; set; }
    }
}