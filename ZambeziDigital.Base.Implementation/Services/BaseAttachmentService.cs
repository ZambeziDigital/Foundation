using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services.Contracts;

namespace ZambeziDigital.Base.Implementation.Services;

public class BaseAttachmentService(IServiceScopeFactory scopeFactory) : BaseService<BaseAttachment, string>(scopeFactory), IBaseAttachmentService;