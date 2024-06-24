using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    public interface IBaseContext : IDisposable
    {

        #region DB Context

        DbContext GetContext();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        #endregion

        #region DB Context Modification Row Object

        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : class;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class;

        Task SetAsAddedAsync<TEntity>(TEntity entity) where TEntity : class;
        Task SetAsModifiedAsync<TEntity>(TEntity entity) where TEntity : class;
        Task SetAsDeletedAsync<TEntity>(TEntity entity) where TEntity : class;

        #endregion

        #region DB Context Modification Rows Objects List

        void SetAsAdded<TEntity>(List<TEntity> entities) where TEntity : class;
        void SetAsModified<TEntity>(List<TEntity> entities) where TEntity : class;
        void SetAsDeleted<TEntity>(List<TEntity> entities) where TEntity : class;

        Task SetAsAddedAsync<TEntity>(List<TEntity> entities) where TEntity : class;
        Task SetAsModifiedAsync<TEntity>(List<TEntity> entities) where TEntity : class;
        Task SetAsDeletedAsync<TEntity>(List<TEntity> entities) where TEntity : class;

        #endregion

        #region DB Context Operations

        int SaveChanges();
        Task<int> SaveChangesAsync();

        #endregion

        #region DB Context Transaction Operations 

        void BeginTransaction();
        Task BeginTransactionAsync();
        int Commit();        
        Task<int> CommitAsync();
        void Rollback();
        Task RollbackAsync();

        #endregion

    }
}
