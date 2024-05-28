namespace ZambeziDigital.Authentication.Data;

public interface IBaseDbContext<TUser> 
    where TUser : IdentityUser, IHasKey<string>, new()
{
    DbSet<TUser> AspNetUsers { get; set; }

    int SaveChanges();
}