namespace ZambeziDigital.Multitenancy.Extensions;


public static class TenantDbAccessGuard
{
    private static bool guard = false;
    public static bool SystemActive { get; set; } = false;

    public static void TurnOn()
    {
        if (SystemActive == true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Be careful, System is active, you can't turn on Tenant Database Guard. Tenant Database Guard is off.");
            Console.ResetColor();
            guard = false;
            return;
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Tenant Database Guard is turning off.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Tenant Database Guard is off.");
        Console.ResetColor();
        guard = true;
    }
    
    public static void TurnOff()
    {
        if (SystemActive == true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Becareful, System is active, you can't turn on Tenant Database Guard. Tenant Database Guard is off.");
            Console.ResetColor();
            guard = false;
            return;
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Tenant Database Guard is turning on.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Tenant Database Guard is on.");
        Console.ResetColor();
        guard = false;
    }
    
    public static bool Guard
    {
        get => guard;
    }
}
