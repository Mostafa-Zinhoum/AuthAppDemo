using AuthAppDemoService.Basics.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Impelmentation
{
    internal class BaseContext : IBaseContext
    {

        #region Set Class

        private DbContext _Context;
        private IDbContextTransaction _Transaction;
        private bool _Disposed = false;


        public BaseContext(DbContext Context)
        {
            _Context = Context;
        }

        #endregion


        #region DB Context

        public DbContext GetContext()
        {
            return _Context;
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _Context.Set<TEntity>();
        }

        #endregion


        #region DB Context Modification Row Object

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Deleted);
        }

        public async Task SetAsAddedAsync<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Added);
            await Task.CompletedTask;
        }

        public async Task SetAsModifiedAsync<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Modified);
            await Task.CompletedTask;
        }

        public async Task SetAsDeletedAsync<TEntity>(TEntity entity) where TEntity : class
        {
            UpdateRowState(entity, EntityState.Deleted);
            await Task.CompletedTask;
        }

        #endregion


        #region DB Context Modification Rows Objects List

        public void SetAsAdded<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Added);
            }

        }

        public void SetAsModified<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Modified);
            }

        }

        public void SetAsDeleted<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Deleted);
            }
        }

        public async Task SetAsAddedAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Added);
            }
            await Task.CompletedTask;
        }

        public async Task SetAsModifiedAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Modified);
            }
            await Task.CompletedTask;
        }

        public async Task SetAsDeletedAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                UpdateRowState(entity, EntityState.Deleted);
            }
            await Task.CompletedTask;
        }

        #endregion


        #region DB Context Operations
        /*
        
        */





        public int SaveChanges()
        {
            return _Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _Context.SaveChangesAsync();
        }

        #endregion


        #region DB Context Transaction Operations 

        public void BeginTransaction()
        {
            _Transaction = _Context.Database.BeginTransaction();
        }
        public async Task BeginTransactionAsync()
        {
            _Transaction = await _Context.Database.BeginTransactionAsync();
        }

        public void Rollback()
        {
            _Transaction.Rollback();
        }
        public async Task RollbackAsync()
        {
            await _Transaction.RollbackAsync();
        }

        public int Commit()
        {
            var saveChanges = _Context.SaveChanges();
            _Transaction.Commit();
            return saveChanges;
        }

        public async Task<int> CommitAsync()
        {
            var saveChangesAsync = await _Context.SaveChangesAsync();
            await _Transaction.CommitAsync();
            return saveChangesAsync;
        }

        #endregion


        #region DB Context Entities Operations

        private void UpdateRowState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            var dbEntityEntry = GetRowEntrySafely(entity, entityState);
        }
        private EntityEntry GetRowEntrySafely<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            var dbEntityEntry = _Context.Entry<TEntity>(entity);
            dbEntityEntry.State = entityState;
            if (dbEntityEntry.State == EntityState.Detached)
            {
                _Context.Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
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
                    _Context.Dispose();
                }
            }
            this._Disposed = true;
        }

        #endregion

    }
}
