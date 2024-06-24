using AuthAppDemoService.Basics.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Impelmentation
{
    internal class ContextsFactor : IContextsFactor
    {

        #region Set Class

        private Dictionary<Type, ServiceDescriptor> _ContextStore;
        private Dictionary<string, string> _EntitiesStore;
        private IDictionary<string, IBaseContext> _ActiveContexts = new Dictionary<string, IBaseContext>();
        IServiceProvider _ServiceProvider;
        private bool _Disposed = false;



        public ContextsFactor(IWorxDB WorxDB)
        {
            _ContextStore = WorxDB.GetContextStore();
            _EntitiesStore = WorxDB.GetEntitiesStore();
            _ServiceProvider = WorxDB.GetServiceProvider();
        }


       

        #endregion

        #region DB Context

        public List<IBaseContext> GetContexts()
        {
            List <IBaseContext> ContextList = new List <IBaseContext>();
            foreach(var ContextObj in _ActiveContexts)
            {
                ContextList.Add(ContextObj.Value);
            }
            return ContextList;
        }

        public IBaseContext GetRepositoryContext<TEntity>() where TEntity : class
        {
            var ContextName = _EntitiesStore.FirstOrDefault(x => x.Key == typeof(TEntity).Name).Value;
            var DBTypeObj = _ContextStore.FirstOrDefault(x => x.Key.Name == ContextName);

            Type DBType = DBTypeObj.Key;
            DbContext ContextObj = (DbContext)_ServiceProvider.GetService(DBType);

            if (!_ActiveContexts.ContainsKey(ContextObj.ContextId.InstanceId.ToString()))
            {
                IBaseContext baseContext = new BaseContext(ContextObj);
                _ActiveContexts.Add(
                    ContextObj.ContextId.InstanceId.ToString(),
                    baseContext
                    );
            }

            return _ActiveContexts.FirstOrDefault(x => x.Key == ContextObj.ContextId.InstanceId.ToString()).Value;

            
        }

        #endregion


        #region DB Context Operations

        public List<int> SaveChanges()
        {
            List<int> Save_Result = new List<int>();
            foreach (var ContextObj in _ActiveContexts)
            {
                Save_Result.Add(ContextObj.Value.SaveChanges());
            }
            return Save_Result;
        }

        public List<Task<int>> SaveChangesAsync()
        {
            List<Task<int>> Save_Result = new List<Task<int>>();
            foreach (var ContextObj in _ActiveContexts)
            {
                Save_Result.Add(ContextObj.Value.SaveChangesAsync());
            }
            return Save_Result;
        }

        #endregion


        #region DB Context Transaction Operations 

        public void BeginTransaction()
        {
            foreach (var ContextObj in _ActiveContexts)
            {
                ContextObj.Value.BeginTransaction();
            }
        }

        public void Rollback()
        {
            foreach (var ContextObj in _ActiveContexts)
            {
                ContextObj.Value.Rollback();
            }
        }

        public List<int> Commit()
        {
            List<int> Save_Result = new List<int>();
            foreach (var ContextObj in _ActiveContexts)
            {
                Save_Result.Add(ContextObj.Value.Commit());
            }
            return Save_Result;
        }

        public List<Task<int>> CommitAsync()
        {
            List<Task<int>> Save_Result = new List<Task<int>>();
            foreach (var ContextObj in _ActiveContexts)
            {
                Save_Result.Add(ContextObj.Value.CommitAsync());
            }
            return Save_Result;
        }
        

        #endregion


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
                    foreach (var ContextDic in _ActiveContexts)
                    {
                        ContextDic.Value.Dispose();
                    }
                    _ActiveContexts = null;
                }
            }
            this._Disposed = true;
        }

        #endregion

    }
}
