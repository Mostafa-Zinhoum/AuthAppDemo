using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    public interface IContextsFactor : IDisposable
    {
        #region DB Context

        List<IBaseContext> GetContexts();
        IBaseContext GetRepositoryContext<TEntity>() where TEntity : class;

        #endregion

        #region DB Context Operations

        List<int> SaveChanges();
        Task<List<int>> SaveChangesAsync();

        #endregion

        #region DB Context Transaction Operations 

        void BeginTransaction();
        Task BeginTransactionAsync();
        void Rollback();
        Task RollbackAsync();
        List<int> Commit();
        Task<List<int>> CommitAsync();

        #endregion

    }
}
