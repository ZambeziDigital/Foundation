namespace ZambeziDigital.Base.Accounting.ZRA;
public class ItemClassificationCode : BaseModel<int>
{
    [JsonPropertyName("itemClsCd")][Key]
    public string Code { get; set; }
    [JsonPropertyName("itemClsNm")]
    public string Name { get; set; }
    [JsonPropertyName("itemClsLvl")]
    public int Level { get; set; }
    [JsonPropertyName("taxTyCd")]
    public string? TaxCode { get; set; } = null;
    [JsonPropertyName("mjrTgYn")]
    public string? mjrTgYn { get; set; } = null;
    [JsonPropertyName("useYn")] public string Active { get; set; } = "Y";
    public override string SearchString => $"{Code} {Name}";
}
