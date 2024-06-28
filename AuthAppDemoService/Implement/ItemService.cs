using AuthAppDemoDB.Models;
using AuthAppDemoDBInfra;
using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Basics.Impelmentation;
using AuthAppDemoService.Basics.Interfaces;
using AuthAppDemoService.Helpers;
using AuthAppDemoService.Interfaces;
using AuthAppDemoService.ServiceDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Implement
{
    public class ItemService : AsyncCrudAppService<Item, ItemDto, long, EntityDto<long>, CreateItemDto, UpdateItemDto>,IItemService//,IApplicationService
    {
        public ItemService(IUnitOfWork unitOfWork,IObjectMapper objectMapper,WorxSession worxSession)
            : base(unitOfWork, objectMapper, worxSession)
            { 
        }
    }
}
