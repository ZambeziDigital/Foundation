namespace ZambeziDigital.Base.Accounting.Statics;

public partial class Codes
{
    public static readonly List<RegistrationType> RegistrationTypes = new List<RegistrationType>
    {
        new RegistrationType {Code = "A", Id = 1, Name = "Automatic", Description = "Automatic – Purchases from another Smart Invoice user. These purchases will be identifiable from the select purchase endpoint (i.e. if the endpoint returns anything, it means these were sold to your TPIN by another Smart Invoice user.)"},
        new RegistrationType {Code = "M", Id = 2, Name = "Manual", Description = "Manual – Purchases from a non-Smart Invoice user These are purchases manually registered onto the system by the taxpayer."}
    };
}