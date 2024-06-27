using AutoMapper;
using AuthAppDemoDB.Models;
using AuthAppDemoService.ServiceDto;
namespace AuthAppDemoService.Helpers
{
    public class WorxMapperProfile : Profile
    {
        public WorxMapperProfile() 
        {
            CreateMap<CreateItemDto, Item>();
            CreateMap<UpdateItemDto, Item>();
            CreateMap<Item, ItemDto>();
        }
    }
}
