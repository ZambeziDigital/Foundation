using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services;
using ZambeziDigital.Base.Implementation.Services.Contracts;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;

public class BaseAttachmentService<TDataContext>(IServiceScopeFactory serviceScopeFactory, TDataContext context)
    : BaseService<BaseAttachment, string, TDataContext>(context), IBaseAttachmentService
    where TDataContext : DbContext
{
    private readonly IHostingEnvironment _environment = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHostingEnvironment>();
    public override async Task<BaseResult<BaseAttachment>> Get(string id, bool cached = false)
    {
        var result = await base.Get(id, cached);

        if (result.Succeeded)
        {
            try
            {
                result.Data.File = null;//System.IO.File.ReadAllBytes(result.Data.Location);
            }
            catch (Exception ex)
            {
                return new BaseResult<BaseAttachment>()
                {
                    Succeeded = false,
                    Message = ex.Message,
                };
            }
        }

        return result;

    }

    public override Task<BaseResult<BaseAttachment>> Create(BaseAttachment entity)
    {
        //Upload the image to the local storage and save the image path to the database
        return Upload(entity);
    }

    public override Task<BaseResult<BaseAttachment>> Update(BaseAttachment entity)
    {
        //Remove old image and add new image from the local storage and update the database with new image path
        return Update(entity.Id, entity);
    }

    public async Task<BaseResult<BaseAttachment>> Upload(BaseAttachment attachment)
    {
        try
        {
            if (attachment.OwnerId is null)
                attachment.OwnerId = "default";
            attachment.Id = Guid.NewGuid().ToString();
            // Specify the directory where you want to save your files
            var uploadDirectory = $"wwwroot/files";

            // Get the content root path of your app
            var contentRootPath = _environment.ContentRootPath;
            string uploadPath = string.Empty;
            uploadPath = Path.Combine(contentRootPath, uploadDirectory);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }


            if (attachment.File is not null && attachment.File.Length > 0)
            {
                byte[] fileBytes = attachment.File; // Your file bytes

                var stream2 = new MemoryStream(fileBytes);
                var formFile = new FormFile(stream2, 0, stream2.Length, "name", "fileName")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = attachment.Type.ToString(),
                };

                var fileName =
                    $"{attachment.OwnerId}_{attachment.Type.ToString()}_{(new Random().Next(100000, 999999).ToString())}_{attachment.Name}";

                // Combine the upload path and the file name
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    formFile.CopyTo(stream);
                }

                attachment.Location = filePath;

                context.Set<BaseAttachment>().Add(attachment);
                await context.SaveChangesAsync();
            }

            return new BaseResult<BaseAttachment>()
            {
                Data = attachment,
            };
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return new BaseResult<BaseAttachment>()
            {
                Data = null,
                Message = e.Message,
                Succeeded = false
            };
        }

    }


    public async Task<BaseResult<BaseAttachment>> Update(string dbImageId, BaseAttachment entityImage)
    {
        //Remove old image and add new image from the local storage and update the database with new image path
        var dbImage = context.Set<BaseAttachment>().FirstOrDefault(a => a.Id == dbImageId);
        if (dbImage is not null)
        {
            if (entityImage.File is not null)
            {
                //remove old image from the local storage
                if (System.IO.File.Exists(dbImage.Location))
                {
                    System.IO.File.Delete(dbImage.Location);
                }

                var attachment = (await Upload(entityImage)).Data;
                dbImage.Location = attachment.Location;
                dbImage.File = attachment.File;
                dbImage.Name = attachment.Name;
                dbImage.Type = attachment.Type;
                dbImage.OwnerId = attachment.OwnerId;
                await context.SaveChangesAsync();
            }
        }
        return new BaseResult<BaseAttachment>()
        {
            Data = dbImage,
            Succeeded = true
        };
    }
}