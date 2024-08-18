global using ZambeziDigital.Base.Contracts.Base;
global using ZambeziDigital.Base.Models.File;

namespace ZambeziDigital.Base.Contracts.File;
public interface IAttachment<TKey> : IBaseModel<TKey> where TKey : IEquatable<TKey>
{
    public byte[]? File { get; set; }
    public string? FileUri { get; }
    public string FileUrl { get; }
    public string? ContentType { get; set; }
    public long? Size { get; set; }
    public string? Location { get; set; }
    public string? OwnerId { get; set; }
    public AttachmentType Type { get; set; }
}