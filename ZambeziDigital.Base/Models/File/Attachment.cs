using System.ComponentModel.DataAnnotations.Schema;
using ZambeziDigital.Base.Contracts.File;

namespace ZambeziDigital.Base.Models.File;

public interface IAttachment : IAttachment<string>
{
    public string Id { get; set; }
    [NotMapped][Newtonsoft.Json.JsonIgnore] public byte[]? File { get; set; }
    [NotMapped][System.Text.Json.Serialization.JsonIgnore] public string? FileUri
    {
        get
        {
            if (File != null) return $"data:image/png;base64,{Convert.ToBase64String(File)}";
            return "";
        }
    }

    [NotMapped][Newtonsoft.Json.JsonIgnore] 
    public string FileUrl => $"https://ctserver.wirepick.dev/api/Image/imageId?imageId={Id}"; //TODO: Change this to the actual url
    //file metadata
    public string? ContentType { get; set; }
    public long? Size { get; set; }
    public string? Location { get; set; }
    public string? OwnerId { get; set; }
    public AttachmentType Type { get; set; }
    
}




public enum AttachmentType
{
    Image,
    Document,
    Other,
}