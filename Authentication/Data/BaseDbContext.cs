using System.Net.Mail;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ZambeziDigital.Authentication.Data;

public partial class BaseDbContext(DbContextOptions<BaseDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // public DbSet<Address> Addresses { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
}