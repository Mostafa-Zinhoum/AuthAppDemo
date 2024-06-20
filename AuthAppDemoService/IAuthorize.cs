using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    public interface IAuthorize
    {
        public string CreateAccessToken(TokenInfoDto param);
        public string RefreshAccessToken();
    }
}
