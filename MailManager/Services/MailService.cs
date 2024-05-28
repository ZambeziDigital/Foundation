using ZambeziDigital.MailManager.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ZambeziDigital.MailManager.Services;

public class MailService(IServiceScopeFactory scopeFactory)  : IMailService
{
    // [HttpGet]
    public async Task<string> SendMail()
    {
        try
        {
            // create email message
            var email = new MimeMessage();
            // email.From.Add(new MailboxAddress("Your Display Name", "from_address@example.com"));
            email.From.Add(new MailboxAddress("Pistis Foundation Developer", "dev@pistis.foundation"));
            email.To.Add(MailboxAddress.Parse("ittai1tumelo@gmail.com"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example HTML Message Body</h1>" };

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("pistis.foundation", 465, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync("dev@pistis.foundation", "dev@pfl23!");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            return "sent";
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error sending mail");
            Console.WriteLine(e.Message);
            return e.Message;
        }
    }
    // await EmailSender.SendPasswordResetLinkAsync(user, email);

    // [HttpPost]
    public async Task<BasicResult> SendMail( MailRequest mailRequest)
    {
        try
        {
            //TODO: Make sure the student has paid for the result mail
            var email = new MimeMessage();
            // email.From.Add(new MailboxAddress("Your Display Name", "from_address@example.com"));
            email.From.Add(new MailboxAddress($"Smart Invoicing - Wirepick Zambia", "dev@pistis.foundation"));
            email.To.Add(MailboxAddress.Parse("ittai1tumelo@gmail.com"));
            // email.To.Add(MailboxAddress.Parse($"{mailRequest.ToEmail}"));
            // email.To.Add(MailboxAddress.Parse("guardian"));
            email.Subject = $"{mailRequest.Subject}";
            
            StringBuilder _html = new StringBuilder();
            _html.Append("<!DOCTYPE html>");
            _html.Append("<html lang=\"en\">");
            _html.Append("<head>");
            _html.Append("<meta charset=\"UTF-8\">");
            _html.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            _html.Append("<title>Document</title>");
            _html.Append(
                "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL\" crossorigin=\"anonymous\"></script>");
            _html.Append(
                "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN\" crossorigin=\"anonymous\">");
            // _html.Append(js);
            // _html.Append(css);
            _html.Append("</head>");
            _html.Append("<body>");
            _html.Append($"<h1>{mailRequest.Body}</h1>");
            _html.Append("</body>");
            _html.Append("</html>");
            string finalHtml = _html.ToString();

            email.Body = new TextPart(TextFormat.Html) { Text = _html.ToString() };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("pistis.foundation", 465, SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync("dev@pistis.foundation", "dev@pfl23!");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            //update the result mail to sent email
            return new BasicResult() { Succeeded = true };
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error sending mail");
            Console.WriteLine(e.Message);
            return new BasicResult() { Succeeded = false, Errors = new List<string> { e.Message } };
        }
    }
    
}

