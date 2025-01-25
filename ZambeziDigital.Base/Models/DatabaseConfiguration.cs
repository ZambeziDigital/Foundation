using ZambeziDigital.Base.Enums;

namespace ZambeziDigital.Base.Models;

public class DatabaseConfiguration
{
    public int Id { get; set; }
    public string PASSWORD { get; set; }
    public string USER_ID { get; set; }
    public string IP_ADDRESS { get; set; }
    public string PORT { get; set; }
    public string DATABASE_NAME { get; set; }
    public DATABASE_TYPE TYPE { get; set; } = DATABASE_TYPE.POSTGRES;
    public bool Customized { get; set; }
}