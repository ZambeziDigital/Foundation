namespace ZambeziDigital.Base.Accounting.Models;

public class User
{
     public required string tpin { get; set; }
     public required string bhfId { get; set; }
     public string userId { get; set; }
     public string userNm { get; set; }
     public string adrs { get; set; }
     public string useYn { get; set; }
     public string regrNm { get; set; }
     public string regrId { get; set; }
     public string modrNm { get; set; }
     public string modrId { get; set; }

     // public ZRAUser(ApplicationUser applicationUser) { 
     //      // this.tpin = applicationUser.Email;
     //      // this.bhfId = applicationUser.BranchId;
     //      this.userId = applicationUser.Name;
     //      this.userNm = applicationUser.Name;
     //      this.adrs = applicationUser.Email;
     //      this.useYn = applicationUser.Active == UserState.Active ? "Y" : "N";
     //      this.regrNm = applicationUser.Roles.Any(x => x.Name.Contains("Admin")) ? "Admin" : "User";
     //      this.regrId = applicationUser.Roles.Any(x => x.Name.Contains("Admin")) ? "Admin" : "User";
     //      this.modrNm = applicationUser.Roles.Any(x => x.Name.Contains("Admin")) ? "Admin" : "User";
     //      this.modrId = applicationUser.Roles.Any(x => x.Name.Contains("Admin")) ? "Admin" : "User";
     // // }
     // public ZRAUser() { }
}
