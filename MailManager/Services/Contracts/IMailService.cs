using ZambeziDigital.BasicAccess.Models;
using ZambeziDigital.MailManager.Models;

namespace ZambeziDigital.MailManager.Services.Contracts;

public interface IMailService
{
    Task<string> SendMail();
    Task<BasicResult> SendMail(MailRequest mailRequest);
}