using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Basics.Interfaces;
using AuthAppDemoService.ServiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Interfaces
{
    public interface IItemService : IAsyncCrudAppService<ItemDto, long, EntityDto<long>, CreateItemDto, UpdateItemDto>, IApplicationService
    {
    }
}
