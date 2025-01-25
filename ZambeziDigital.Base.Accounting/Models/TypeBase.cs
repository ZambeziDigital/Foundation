global using ZambeziDigital.Base.Contracts.Base;
global using ZambeziDigital.Base.Models;
using ZambeziDigital.Functions.Helpers;

namespace ZambeziDigital.Base.Accounting.ZRA.Models;

public class TypeBase : BaseModel<int>
{
    [Searchable] public string Code { get; set; }
    public int Id { get; set; }
    [Searchable] public override string Name { get; set; }
    [Searchable] public string? Description { get; set; }
    [Searchable] public string? Remark { get; set; }
    public int SortOrder => Id;
}