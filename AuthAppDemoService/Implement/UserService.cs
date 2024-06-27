using AuthAppDemoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthAppDemoLog;

namespace AuthAppDemoService
{
    [LogAspect]
    internal class UserService : IUserService
    {
        public LoginDto GetUser(GetUserRequest Param)
        {
            using (AuthDemo01Context db = new AuthDemo01Context())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == Param.UserId);
                if (user != null)
                    return new LoginDto
                    {
                        UserId = user.Id.ToString(),
                        UserName = user.Name
                    };

                return null;
            }
        }

        public LoginDto Login(LoginRequest Param)
        {
            using (AuthDemo01Context db = new AuthDemo01Context())
            {
                var user = db.Users.FirstOrDefault(x => x.Name == Param.LoginName && x.Password == Param.LoginPassword);
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
}
