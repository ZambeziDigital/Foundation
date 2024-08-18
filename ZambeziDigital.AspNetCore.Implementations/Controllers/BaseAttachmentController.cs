using Microsoft.AspNetCore.Mvc;
using ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services.Contracts;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.AspNetCore.Implementations.Controllers;


public class BaseAttachmentController(IBaseAttachmentService baseAttachmentService)
    : BaseController<BaseAttachment, string>(baseAttachmentService)
{
    [HttpGet("file/{fileId}")]
    public virtual async Task<ActionResult> Download(string fileId)
    {
        try
        {
            if (string.IsNullOrEmpty(fileId)) return BadRequest("attachment Id can not be null");
            var attachmentResult = (await baseAttachmentService.Get(fileId));
            if (attachmentResult.Succeeded)
                return File(attachmentResult.Data.File, attachmentResult.Data.ContentType);
            else return NotFound(
                    new BaseResult()
                    {
                        Succeeded = false,
                        Message = "File not found"
                    }
                );
        }
        catch (Exception ex)
        {
            return Ok(new BaseResult()
            {
                Succeeded = false,
                Message = ex.Message
            });
        }
    }

}