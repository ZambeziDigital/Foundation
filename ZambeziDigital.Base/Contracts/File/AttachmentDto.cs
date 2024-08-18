namespace ZambeziDigital.Base.Contracts.File;

public interface IAttachmentDto
{
    public byte[] File { get; set; }
    //file metadata
    public string ContentType { get; set; } 
    public long Size { get; set; }
    public string? OwnerId { get; set; }
    public string OwnerType { get; set; }
    public AttachmentType Type { get; set; }
}
