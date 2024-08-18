using ZambeziDigital.AspNetCore.Implementations.Generics.Services;
using ZambeziDigital.Base.Accounting.Models;
using ZambeziDigital.Management.Server.Data;

namespace ZambeziDigital.Management.Server;

public class SAASService(DataContext context) : BaseService<SAAS, int, DataContext>(context), ISAASService 
{
    
}