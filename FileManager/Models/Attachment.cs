using System.ComponentModel.DataAnnotations.Schema;
using ZambeziDigital.BasicAccess.Contracts;

namespace ZambeziDigital.FileManager.Models;

public class Attachment : IHasKey<int>
{
    public int Id { get; set; }
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
    public string? ContentType { get; set; } = string.Empty!;
    public long? Size { get; set; }
    public string? Location { get; set; } = string.Empty!;
    public string? OwnerId { get; set; }
    public AttachmentType Type { get; set; }
    
}


public enum AttachmentType
{
    Image,
    Document,
    Other,
}