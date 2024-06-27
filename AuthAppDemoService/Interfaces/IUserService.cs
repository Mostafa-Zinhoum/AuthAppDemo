using AuthAppDemoLog;
using AuthAppDemoService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    [LogAspect]
    public interface IUserService
    {
        LoginDto Login(LoginRequest Param);
        LoginDto GetUser(GetUserRequest Param);
    }
}
