using System.Data;
using ZambeziDigital.Base.Enums;

namespace ZambeziDigital.Base.Models;

public static partial class Environment
{
    public static string DATABASE_PORT { get; set; } = "5432";
    public static string DATABASE_IP { get; set; } = "localhost";
    public static ENV ENV { get; set; } = ENV.DEVELOPMENT;
    public static DATABASE_TYPE DATABASE_TYPE { get; set; } = DATABASE_TYPE.POSTGRES;
    public static string SERVER_PORT => "0000";
    public static string DATABASE_NAME { get; set; } = "Template";
    public static string DATABASE_USER { get; set; } = "postgres";
    public static string DATABASE_PASSWORD { get; set; } = "MySQLDatabase1.";
    public static string PRODUCTION_SERVER_IP => "195:200.14.221";
    public static string DEVELOPMENT_SERVER_IP { get; set; } = "localhost";
    public static string ESSENTIALS_SERVER_URL { get; set; } = "https://essentials.zambezidigital.dev";
    public static string IP_ADDRESS => ENV switch
    {
        ENV.PRODUCTION => PRODUCTION_SERVER_IP,
        ENV.DEVELOPMENT => DEVELOPMENT_SERVER_IP,
        _ => ""
    };

    private static string DOMAIN => ENV switch
    {
        ENV.PRODUCTION => "zambezidigital.com",
        ENV.DEVELOPMENT => "zambezidigital.dev",
        _ => ""
    };
    public static string API_URL => $"{SERVER_URL}";
    public static string API_URL2 => $"https://{DEVELOPMENT_SERVER_IP}";

    public static string VSDC_PORT => ENV switch
    {
        ENV.PRODUCTION => "8080/zraprodvsdc/",
        ENV.DEVELOPMENT => "8080/zrasandboxvsdc/",
        _ => ""
    };

    public static string VSDC_URL => ENV switch
    {
        ENV.PRODUCTION => $"http://{PRODUCTION_SERVER_IP}:{VSDC_PORT}",
        ENV.DEVELOPMENT => $"http://{DEVELOPMENT_SERVER_IP}:{VSDC_PORT}",
        _ => ""
    };

    public static string SERVER_URL => ENV switch
    {
        ENV.PRODUCTION => $"https://{IP_ADDRESS}:{SERVER_PORT}",
        ENV.DEVELOPMENT => $"https://{IP_ADDRESS}:{SERVER_PORT}",
        _ => ""
    };
    public static string WEB_URL => ENV switch
    {
        ENV.PRODUCTION => $"https://finance.{DOMAIN}",
        ENV.DEVELOPMENT => $"https://finance.{DOMAIN}",
        _ => ""
    };
    public static string DEFAULT_TPIN => "0000000000";
    public static string PDF_GENERATOR_URL => $"https://topdf.{DOMAIN}/generate_pdf";
    public static string DEFAULT_BRANCH_CODE => "000";
    public static string DEFAULT_CONNECTION_STRING => DATABASE_TYPE switch
    {
        DATABASE_TYPE.MYSQL => $"Server={DATABASE_IP};Port=3306;Database={DATABASE_NAME};Uid={DATABASE_USER};Pwd={DATABASE_PASSWORD};SSL Mode=None;Pooling=true;Connect Timeout=28800;Command Timeout=28800;AllowPublicKeyRetrieval=True;AllowZeroDateTime=True;ConvertZeroDateTime=True",
        DATABASE_TYPE.POSTGRES => $"Host={DATABASE_IP};Database={DATABASE_NAME};Password={DATABASE_PASSWORD};Port={DATABASE_PORT};User Id={DATABASE_USER};Timeout=10;",// connect_timeout=10  sslmode=prefer",//$"Server={IP_ADDRESS};Port=5432;Database={DATABASE_NAME};User Id={USER};Password={PASSWORD};",
        DATABASE_TYPE.SQLITE => $"Data Source={DATABASE_NAME}.db",
        DATABASE_TYPE.SQL_SERVER => $"Server={DATABASE_IP};Database={DATABASE_NAME};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};",
        _ => throw new DataException("Database type configurations required"),
    };

    public static string CONNECTION_STRING(DATABASE_TYPE TYPE, string PASSWORD, string USER, string IP_ADDRESS, string PORT, string DATABASE_NAME) => TYPE switch
    {
        DATABASE_TYPE.MYSQL => $"Server={DATABASE_IP};Port={DATABASE_PORT};Database={DATABASE_NAME};Uid={USER};Pwd={PASSWORD};",
        DATABASE_TYPE.POSTGRES => $"Host={DATABASE_IP};Database={DATABASE_NAME};Password={DATABASE_PASSWORD};Port={DATABASE_PORT};User Id={DATABASE_USER};Timeout=10;",// connect_timeout=10  sslmode=prefer",//$"Server={IP_ADDRESS};Port=5432;Database={DATABASE_NAME};User Id={USER};Password={PASSWORD};",
        DATABASE_TYPE.SQLITE => $"Data Source={DATABASE_NAME}.db",
        DATABASE_TYPE.SQL_SERVER => $"Server={DATABASE_IP};Database={DATABASE_NAME};User Id={USER};Password={PASSWORD};",
        _ => throw new DataException("Database type configurations required"),
    };

    public static string CONNECTION_STRING(DatabaseConfiguration tenantDatabaseConfiguration)
    {
        try
        {
            return CONNECTION_STRING(tenantDatabaseConfiguration.TYPE, tenantDatabaseConfiguration.PASSWORD,
                tenantDatabaseConfiguration.USER_ID, tenantDatabaseConfiguration.IP_ADDRESS,
                tenantDatabaseConfiguration.PORT, tenantDatabaseConfiguration.DATABASE_NAME);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }

    public static DatabaseConfiguration? TEMP_DatabaseConfiguration { get; set; }
}


public enum ENV
{
    PRODUCTION,
    DEVELOPMENT,
    DEEVELOPMENT_ITTAI,
}

