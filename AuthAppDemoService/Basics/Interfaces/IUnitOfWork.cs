using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    public interface IUnitOfWork
    {

        #region DB Context

        List<IBaseContext> GetContexts();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        #endregion

        #region DB Context Operations

        List<int> SaveChanges();

        List<Task<int>> SaveChangesAsync();

        #endregion


        
        #region DB Context Transaction Operations 

        void BeginTransaction();
        void Rollback();
        List<int> Commit();
        List<Task<int>> CommitAsync();

        #endregion
       
    }
}
