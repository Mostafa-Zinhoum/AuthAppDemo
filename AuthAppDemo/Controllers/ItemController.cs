using AuthAppDemoLog;
using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Interfaces;
using AuthAppDemoService.ServiceDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthAppDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [LogAspect]
    //[Authorize]
    public class ItemController : ControllerBase
    {
        protected IItemService itemService;
        public ItemController(IItemService _itemService) 
        {
            itemService = _itemService;
        }

        [HttpPost]
        public async Task<ItemDto> AddItem(CreateItemDto param)
        {
            return await itemService.CreateAsync(param);
        }

        [HttpPost]
        public async Task<ItemDto> UpdItem(UpdateItemDto param)
        {
            return await itemService.UpdateAsync(param);
        }

        [HttpPost]
        public async Task<ItemDto> GetItem(EntityDto<long> param)
        {
            return await itemService.GetAsync(param);
        }
    }
}
