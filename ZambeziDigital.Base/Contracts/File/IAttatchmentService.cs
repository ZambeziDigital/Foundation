global using ZambeziDigital.Base.DTOs.Auth;
global using ZambeziDigital.Base.Models.File;
global using ZambeziDigital.Base.Services.Contracts;

// using  Attachment = IM.Shared.Models.Isolated.Attachment;
namespace ZambeziDigital.Base.Contracts.File;

public interface IAttachmentService<TAttachment, TAttachmentDto, TKey> :  
    IBaseService<TAttachment,TAttachmentDto, TKey> 
    where TAttachment : class, IAttachment<string>, IHasKey<TKey>, new()
    where TAttachmentDto : class, IAttachmentDto, new()
    where TKey : IEquatable<TKey>
{
    Task<TAttachment> Update(string dbImageId, TAttachmentDto entityImage);
}
