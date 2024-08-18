namespace Shared.Models.ZRA.Models;

public class SalesResponse : SalesResponse<object>;
public class SalesResponse<T> where T : class
{
        public string resultCd { get; set; }
        public string resultMsg { get; set; }
        public string? resultDt { get; set; }
        public T data { get; set; }
        public bool IsSuccess => resultCd == "000";
}

public class SaleSignature
{
        public int rcptNo { get; set; }
        public string intrlData { get; set; }
        public string rcptSign { get; set; }
        public string vsdcRcptPbctDate { get; set; }
        public string sdcId { get; set; }
        public string mrcNo { get; set; }
        public string qrCodeUrl { get; set; }
        // public override string SearchString => $"{rcptNo} {intrlData} {rcptSign} {vsdcRcptPbctDate} {sdcId} {mrcNo} {qrCodeUrl}" +
                                               // $"";
}


