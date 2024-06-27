using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Helpers
{
    public class LoginDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }

    }
    public class LoginRequest
    {
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
    }

    public class GetUserRequest
    {

        public long UserId { get; set; }
    }
}
