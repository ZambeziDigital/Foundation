using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using ZambeziDigital.Base.Contracts.File;
using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;

[ApiController]
[EnableCors]
[Route("api/[controller]")]
public class FileController<TAttachment, TContext, TKey, TAttachmentDto>(TContext context, IAttachmentService<TAttachment, TAttachmentDto, string> service) :
    BaseController<TAttachment, TAttachmentDto, TKey>(service as IBaseService<TAttachment, TAttachmentDto, TKey>)
    where TAttachmentDto : class, IAttachment<TKey>, IAttachmentDto, new()
    where TAttachment : class, IAttachment<TKey>, IAttachment<string>, new()
    where TContext : DbContext, IHasAttachmentDbContext<TAttachment, TKey>
    where TKey : IEquatable<TKey>
{

}