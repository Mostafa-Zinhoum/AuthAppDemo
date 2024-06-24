using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    internal interface IWorxDB : IDisposable
    {
        public Dictionary<Type, ServiceDescriptor> GetContextStore();
        public Dictionary<string, string> GetEntitiesStore();
        public IServiceProvider GetServiceProvider();

    }
}
