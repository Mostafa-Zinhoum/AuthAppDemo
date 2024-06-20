using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoLog
{
    public class CustomLogState
    {
        public string Message { get; set; }
        public IDictionary<string, string> RequestHeaders { get; set; }
    }
}
