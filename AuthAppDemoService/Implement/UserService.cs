using AuthAppDemoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthAppDemoLog;
using AuthAppDemoService.Helpers;
using AuthAppDemoService.Basics.Interfaces;
using AuthAppDemoService.Basics.Impelmentation;
using AuthAppDemoDBInfra;

namespace AuthAppDemoService
{
    [LogAspect]
    internal class UserService : ApplicationService, IUserService
    {

        public UserService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public async Task<LoginDto> GetUser(GetUserRequest Param)
        {

            var user = await UnitOfWork.Repository<UserInfo>().GetSingle(x => x.Id == Param.UserId);
            if (user != null)
                return new LoginDto
                {
                    UserId = user.Id.ToString(),
                    UserName = user.Name
                };

            return null;
        }

        public async Task<LoginDto> Login(LoginRequest Param)
        {
            var user = await UnitOfWork.Repository<UserInfo>().GetSingle(x => x.Name == Param.LoginName && x.Password == Param.LoginPassword);
            if (user != null)
                return new LoginDto
                {
                    UserId = user.Id.ToString(),
                    UserName = user.Name
                };

            return null;
        }
    }
}
