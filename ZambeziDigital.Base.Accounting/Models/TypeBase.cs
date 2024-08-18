global using ZambeziDigital.Base.Contracts.Base;
global using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Accounting.ZRA.Models;

public class TypeBase: BaseModel<int>, ISearchable 
{
    public string Code { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Remark { get; set; }
    public int SortOrder => Id;
    public override string SearchString => Code + Name + Description + Remark;
}