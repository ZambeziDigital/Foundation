namespace ZambeziDigital.Base.Models;

public class MailRequest
{
    public List<string> ToEmails { get; set; }
    public List<string> CcEmails { get; set; }
    public List<string> BccEmails { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}