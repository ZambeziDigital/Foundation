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
    /// <summary>
    /// "pdf" => "bi-file-earmark-pdf text-danger",
    // "doc" or "docx" => "bi-file-earmark-word text-primary",
    // "xls" or "xlsx" => "bi-file-earmark-excel text-success",
    // "ppt" or "pptx" => "bi-file-earmark-ppt text-warning",
    // "jpg" or "jpeg" or "png" or "gif" => "bi-file-earmark-image text-info",
    // "zip" or "rar" => "bi-file-earmark-zip text-secondary",
    // _ => "bi-file-earmark text-muted"
    /// </summary>
    pdf, doc, docx, xls, xlsx, ppt, pptx, jpg, jpeg, png, gif, zip, rar, other = 100, Image, Document, Other

}