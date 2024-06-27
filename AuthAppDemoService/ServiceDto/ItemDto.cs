using AuthAppDemoService.Basics.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.ServiceDto
{
    public class ItemDto : IEntityDto<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class CreateItemDto : CreateEntityDto<long>
    {
        public string Name { get; set; }
    }
    public class UpdateItemDto : UpdateEntityDto<long>
    {
        public string Name { get; set; }
    }
}
