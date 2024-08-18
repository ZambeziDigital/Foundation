namespace ZambeziDigital.Base.Services.Contracts.Mail;

public interface IMailService<TResult>
{
    Task<string> SendMail();
    Task<TResult> SendMail(MailRequest mailRequest);
    Task<TResult> SendMail(
        MailRequest mailRequest,
        bool retires = false, 
        int maxRetries = 3, 
        int delay = 1000);
}



