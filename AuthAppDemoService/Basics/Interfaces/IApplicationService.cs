using AuthAppDemoService.Basics.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    // Summary:
    //     This interface must be implemented by all application services to identify them
    //     by convention.
    public interface IApplicationService
    {
    }
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput> : IApplicationService where TEntityDto : IEntityDto<TPrimaryKey> where TUpdateInput : IEntityDto<TPrimaryKey> where TGetInput : IEntityDto<TPrimaryKey> where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> GetAsync(TGetInput input);

        Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input);

        Task<TEntityDto> CreateAsync(TCreateInput input);

        Task<TEntityDto> UpdateAsync(TUpdateInput input);

        Task DeleteAsync(TDeleteInput input);
    }

    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput> : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>, IApplicationService where TEntityDto : IEntityDto<TPrimaryKey> where TUpdateInput : IEntityDto<TPrimaryKey> where TGetInput : IEntityDto<TPrimaryKey>
    {
    }
    public interface IAsyncCrudAppService<TEntityDto, TPrimaryKey, in TGetAllInput, in TCreateInput, in TUpdateInput> : IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>, IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>, EntityDto<TPrimaryKey>>, IApplicationService where TEntityDto : IEntityDto<TPrimaryKey> where TUpdateInput : IEntityDto<TPrimaryKey>
    {
    }

}
