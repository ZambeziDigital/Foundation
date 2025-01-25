using ZambeziDigital.Base.Implementation.Services;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Services.Contracts;
using ZambeziDigital.Functions.Helpers;

namespace ExampleApp;

public class ExampleModel : BaseModel<int>
{
    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public ExampleEnum Name2 { get; set; }
    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public bool Mode { get; set; }
    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public int Mode2 { get; set; }
    [PassOnCreate]
    [DigitalDetail]
    [DigitalColumn]
    public double Mode3 { get; set; }
}

public enum ExampleEnum
{
    Example1,
    Example2
}

public class ExampleService(IServiceScopeFactory serviceProviderFactory)
    : BaseService<ExampleModel, int>(serviceProviderFactory), IBaseService<ExampleModel, int>
{
    public override async Task<BaseListResult<ExampleModel>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null,
        bool reversed = false)
    {
        return new BaseListResult<ExampleModel>
        {
            Data = new()
            {
                new ExampleModel
                {
                    Id = 1,
                    Name2 = ExampleEnum.Example1,
                    Mode = true,
                    Mode2 = 1,
                    Mode3 = 1.1
                },
                new ExampleModel
                {
                    Id = 2,
                    Name2 = ExampleEnum.Example2,
                    Mode = false,
                    Mode2 = 2,
                    Mode3 = 2.2
                },
                new ExampleModel
                {
                    Id = 3,
                    Name2 = ExampleEnum.Example1,
                    Mode = true,
                    Mode2 = 3,
                    Mode3 = 3.3
                }
            },
            TotalCount = 0,
            CurrentPage = 0,
            PageSize = 0
        };
        
    }

    public override async Task<BaseResult<ExampleModel>> Create(ExampleModel dto)
    {
        return new BaseResult<ExampleModel>
        {
            Data = new ExampleModel
            {
                Id = 1,
                Name2 = ExampleEnum.Example1,
                Mode = true,
                Mode2 = 1,
                Mode3 = 1.1
            },
            Succeeded = true
        };
    }
    
    
}