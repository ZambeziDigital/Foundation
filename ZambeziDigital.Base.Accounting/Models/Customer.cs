namespace ZambeziDigital.Base.Accounting.ZRA;

    public class Customer
    {
        public string tpin { get; set; }
        public string bhfId { get; set; }
        public string? custNo { get; set; }
        public string? custTpin { get; set; }
        public string? custNm { get; set; }
        public string? adrs { get; set; }
        public string? telNo { get; set; }
        public string? email { get; set; }
        public string? faxNo { get; set; }
        public string? useYn { get; set; }
        public string? remark { get; set; }
        public string? regrId { get; set; }
        public string? regrNm { get; set; }
        public string? modrId { get; set; }
        public string? modrNm { get; set; }

        public int? BranchId { get; set; }
    }