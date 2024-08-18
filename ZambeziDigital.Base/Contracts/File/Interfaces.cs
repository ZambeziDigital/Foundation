global using Microsoft.EntityFrameworkCore;

namespace ZambeziDigital.Base.Contracts.File;

public interface IHasAttachmentDbContext<TAttachment, TKey> 
    where TAttachment : class, IAttachment<TKey>
    where TKey : IEquatable<TKey>
{
    DbSet<TAttachment> Attachments { get; set; }
}