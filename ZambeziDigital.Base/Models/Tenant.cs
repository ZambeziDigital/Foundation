namespace ZambeziDigital.Base.Models;
public class Tenant : BaseModel<int>
{
    public int? DatabaseConfigurationId { get; set; }
    public string? Logo { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int? SubscriptionId { get; set; }
    public DatabaseConfiguration? DatabaseConfiguration { get; set; }
    public string ConnectionString => Environment.CONNECTION_STRING(DatabaseConfiguration);
}