global using ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;
global using ZambeziDigital.Base.Models;
global using ZambeziDigital.Base.Implementation.Services.Contracts;
using ZambeziDigital.Base.Accounting.Models;

namespace ZambeziDigital.Management.Server.Controllers;

public class SaasController(ISAASService service)
    : BaseController<SAAS, int>(service);