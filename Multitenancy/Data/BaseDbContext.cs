using System.Net.Mail;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ZambeziDigital.Multitenancy.Data;

public partial class BaseDbContext(DbContextOptions<BaseDbContext> options) :  IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantSubscription> CompanySubscriptions { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    // public DbSet<Address> Addresses { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
}