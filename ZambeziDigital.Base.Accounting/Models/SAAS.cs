namespace ZambeziDigital.Base.Accounting.Models;

public class SAAS : BaseModel<int>
{
    public string APIServerUrl { get; set; }
    public int Tenants { get; set; }
    public int Users { get; set; }
    // public int APIKeys { get; set; }
}