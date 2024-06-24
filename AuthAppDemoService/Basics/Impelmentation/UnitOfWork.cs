using AuthAppDemoService.Basics.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Impelmentation
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Set Class

        private IContextsFactor _ContextsFactor;
        private IBaseContext _Context;
        private Hashtable _Repositories = new Hashtable();
        private bool _Disposed = false;

        public UnitOfWork(IContextsFactor ContextsFactor)
        {
            _ContextsFactor = ContextsFactor;
        }

        #endregion
        
        #region DB Context

       
        public List<IBaseContext> GetContexts()
        {
            return _ContextsFactor.GetContexts();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_Repositories.Contains(typeof(TEntity)))
            {
                _Context = _ContextsFactor.GetRepositoryContext<TEntity>();
                _Repositories.Add(typeof(TEntity), new Repository<TEntity>(_Context));
            }
            return (IRepository<TEntity>)_Repositories[typeof(TEntity)];
        }

        #endregion

        
        #region DB Context Operations

        public List<int> SaveChanges()
        {
            return _ContextsFactor.SaveChanges();
        }

        public List<Task<int>> SaveChangesAsync()
        {
            return _ContextsFactor.SaveChangesAsync();
        }

        #endregion
        

        #region DB Context Transaction Operations 

        public void BeginTransaction()
        {
            _ContextsFactor.BeginTransaction();
        }

        public void Rollback()
        {
            _ContextsFactor.Rollback();
        }

        public List<int> Commit()
        {
            return _ContextsFactor.Commit();
        }

        public Task<List<int>> CommitAsync()
        {
            return await _ContextsFactor.CommitAsync();
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
                    _ContextsFactor.Dispose();
                }
            }
            this._Disposed = true;
        }

        #endregion

    }
}
