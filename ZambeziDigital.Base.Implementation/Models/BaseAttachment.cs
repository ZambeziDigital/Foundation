using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Contracts.File;
using ZambeziDigital.Base.Implementation.DTOs;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Models.File;

namespace ZambeziDigital.Base.Implementation.Models;

public class BaseAttachment : BaseModel<string>, IAttachment<string>, ISearchable
{
    public string? FileUri { get; }
    [NotMapped]
    public string FileUrl
    {
        get
        {
            string location = this.Location;
            string str;
            if (location == null)
            {
                str = (string)null;
            }
            else
            {
                string[] strArray = location.Split('/');
                str = strArray[strArray.Length - 1];
                
                strArray = str.Split('\\');
                str = strArray[strArray.Length - 1];
            }
            return "files/" + str;
        }
    }
    [NotMapped][JsonIgnore] public byte[]? File { get; set; }
    //file metadata
    public string? ContentType { get; set; } = string.Empty!;
    public long? Size { get; set; }
    public string? Location { get; set; } = string.Empty!;
    public string? OwnerId { get; set; }
    public AttachmentType Type { get; set; }
    public string SearchString => OwnerId + Location + Type + Name;
}
