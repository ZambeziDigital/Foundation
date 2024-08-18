using ZambeziDigital.Base.Contracts.File;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models.File;

namespace ZambeziDigital.Base.Implementation.DTOs;

public class AttachmentDto : IAttachmentDto
{
    public byte[] File { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string? OwnerId { get; set; }
    public string OwnerType { get; set; }
    public string? Location { get; set; }
    public string? Name { get; set; }
    public AttachmentType Type { get; set; }
}