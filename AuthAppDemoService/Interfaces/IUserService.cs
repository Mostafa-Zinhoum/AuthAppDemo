using AuthAppDemoLog;
using AuthAppDemoService.Basics.Interfaces;
using AuthAppDemoService.Helpers;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    [LogAspect]
    public interface IUserService : IApplicationService
    {
        Task<LoginDto> Login(LoginRequest Param);
        Task<LoginDto> GetUser(GetUserRequest Param);
    }
}
