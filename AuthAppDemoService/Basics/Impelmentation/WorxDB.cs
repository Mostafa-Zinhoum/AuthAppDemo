using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthAppDemoService.Basics.Interfaces;

namespace AuthAppDemoService.Basics.Impelmentation
{
    internal class WorxDB : IWorxDB
    {

        #region Set Class

        private Dictionary<Type, ServiceDescriptor> _ContextStore;
        private Dictionary<string, string> _EntitiesStore;
        IServiceCollection _DBServices;
        IServiceCollection _SYSServices;
        IServiceProvider _ServiceProvider;
        private bool _Disposed = false;

        public WorxDB(IServiceCollection SYSServices, IServiceCollection DBServices)
        {
            _DBServices = DBServices;
            _SYSServices = SYSServices;
            _ServiceProvider = DBServices.BuildServiceProvider();
            Set_ContextStore();
        }

        private void Set_ContextStore()
        {
            _ContextStore = new Dictionary<Type, ServiceDescriptor>();

            foreach (var Serv in _DBServices.Where(x => x.ImplementationType != null))
            {

                Type DBType = Serv.ServiceType;

                

                _ContextStore.Add(
                    DBType,
                    Serv
                    );
            }

            Set_EntitiesStore();
        }

        private void Set_EntitiesStore()
        {

            _EntitiesStore = new Dictionary<string, string>();

            foreach (var DBDic in _ContextStore)
            {
                
                    Type DBType = DBDic.Key;
                //DbContext DB = (DbContext)_ServiceProvider.GetService(DBType);
                var DBservice = _ServiceProvider.GetService(DBType);
                if (DBservice is DbContext DB)
                {

                    var EntitiesList = DB.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
                    foreach (var ent in EntitiesList)
                    {
                        _EntitiesStore.Add(ent.Name, DB.GetType().Name);
                    }
                }
            }
        }

        #endregion


        public Dictionary<Type, ServiceDescriptor> GetContextStore()
        {
            return _ContextStore;
        }

        public Dictionary<string, string> GetEntitiesStore()
        {
            return _EntitiesStore;
        }

        public IServiceProvider GetServiceProvider()
        {
            return _ServiceProvider;
        }


        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing)
                {
                    _ContextStore = null;
                    _EntitiesStore = null;
                }
            }
            this._Disposed = true;
        }

        #endregion

    }
}
