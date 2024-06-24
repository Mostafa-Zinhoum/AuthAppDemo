using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AuthAppDemoService.Basics.Interfaces;
using AuthAppDemoService.Basics.Dtos;

namespace AuthAppDemoService.Basics.Impelmentation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        #region Set Class

        private readonly IBaseContext _Context;
        private readonly DbSet<TEntity> _DBSet;
        private bool _Disposed = false;


        public Repository(IBaseContext context)
        {
            this._Context = context;
            this._DBSet = context.Set<TEntity>();
        }

        #endregion



        #region Get And Search Filters Operations

        public async Task<List<TEntity>> GetAll()
        {
            return await _DBSet.ToListAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> BWhere)
        {
            return await _DBSet.Where(BWhere).ToListAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> BWhere, Expression<Func<TEntity, Int64>> KeyOrder, Enum_DB_Order_By orderBy = Enum_DB_Order_By.Ascending, int Page_Index = 0, int Page_Count = 0)
        {
            List<TEntity> Result;
            if (orderBy == Enum_DB_Order_By.Ascending)
            {
                if (Page_Count > 0)
                {
                    Result = await _DBSet.Where(BWhere).OrderBy(KeyOrder).Skip((Page_Index - 1) * Page_Count).Take(Page_Count).ToListAsync();
                }
                else
                {
                    Result = await _DBSet.Where(BWhere).OrderBy(KeyOrder).ToListAsync();
                }
            }
            else
            {
                if (Page_Count > 0)
                {
                    Result = await _DBSet.Where(BWhere).OrderByDescending(KeyOrder).Skip((Page_Index - 1) * Page_Count).Take(Page_Count).ToListAsync();
                }
                else
                {
                    Result = await _DBSet.Where(BWhere).OrderByDescending(KeyOrder).ToListAsync();
                }
            }

            return Result;
        }

        public async Task<Pagination_Result> GetPagination(int Page_Index = 0, int Page_Count = 0)
        {
            List<TEntity> Rows;

            if (Page_Count > 0)
            {
                Rows = await _DBSet.Skip((Page_Index - 1) * Page_Count).Take(Page_Count).ToListAsync();
            }
            else
            {
                Rows = await _DBSet.ToListAsync();
            }

            if (Page_Count == 0)
            {
                Page_Index = 1;
                Page_Count = Rows.Count;
            }
            Pagination_Result Result = new Pagination_Result(Rows, Page_Index, Page_Count, Rows.Count);

            return Result;
        }

        public async Task<Pagination_Result> GetPagination(Expression<Func<TEntity, bool>> BWhere, Expression<Func<TEntity, Int64>> KeyOrder, Enum_DB_Order_By orderBy = Enum_DB_Order_By.Ascending, int Page_Index = 0, int Page_Count = 0)
        {
            List<TEntity> Rows;
            if (orderBy == Enum_DB_Order_By.Ascending)
            {
                if (Page_Count > 0)
                {
                    Rows = await _DBSet.Where(BWhere).OrderBy(KeyOrder).Skip((Page_Index - 1) * Page_Count).Take(Page_Count).ToListAsync();
                }
                else
                {
                    Rows = await _DBSet.Where(BWhere).OrderBy(KeyOrder).ToListAsync();
                }
            }
            else
            {
                if (Page_Count > 0)
                {
                    Rows = await _DBSet.Where(BWhere).OrderByDescending(KeyOrder).Skip((Page_Index - 1) * Page_Count).Take(Page_Count).ToListAsync();
                }
                else
                {
                    Rows = await _DBSet.Where(BWhere).OrderByDescending(KeyOrder).ToListAsync();
                }
            }

            if (Page_Count == 0)
            {
                Page_Index = 1;
                Page_Count = Rows.Count;
            }
            Pagination_Result Result = new Pagination_Result(Rows, Page_Index, Page_Count, Rows.Count);

            return Result;
        }


        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> BWhere)
        {
            return await _DBSet.FirstOrDefaultAsync(BWhere);
        }

        public async Task<TEntity> FirstOrDefault()
        {
            return await _DBSet.FirstOrDefaultAsync();
        }
        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> BWhere)
        {
            return await _DBSet.FirstOrDefaultAsync(BWhere);
        }
        public async Task<TEntity> LastOrDefault(Expression<Func<TEntity, dynamic>> keySelector)
        {
            return await _DBSet.OrderByDescending(keySelector).FirstOrDefaultAsync();
        }
        public async Task<TEntity> LastOrDefault(Expression<Func<TEntity, dynamic>> keySelector, Expression<Func<TEntity, bool>> BWhere)
        {
            return await _DBSet.OrderByDescending(keySelector).FirstOrDefaultAsync(BWhere);
        }

        #endregion

        #region Numberic Operations

        public async Task<Int64> Max(Expression<Func<TEntity, Int64>> KeyMax)
        {
            Int64? Result = await _DBSet.MaxAsync(KeyMax);
            return Result == null ? 0 : (Int64)Result;
        }

        public async Task<Int64> Max(Expression<Func<TEntity, Int64>> KeyMax, Expression<Func<TEntity, bool>> BWhere)
        {
            Int64? Result = await _DBSet.Where(BWhere).MaxAsync(KeyMax);
            return Result == null ? 0 : (Int64)Result;
        }

        public async Task<Int64> Count()
        {
            return await _DBSet.CountAsync();
        }
        public async Task<Int64> Count(Expression<Func<TEntity, bool>> BWhere)
        {
            return await _DBSet.CountAsync(BWhere);
        }

        #endregion

        #region Row Operations

        public async Task<TEntity> Insert(TEntity entity)
        {
            await _Context.SetAsAddedAsync(entity);
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            await _Context.SetAsModifiedAsync(entity);
            return entity;
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            await _Context.SetAsDeletedAsync(entity);
            return entity;
        }

        #endregion

        #region Rows List Operations

        public async Task<List<TEntity>> Insert(List<TEntity> entities)
        {
            await _Context.SetAsAddedAsync(entities);
            return entities;
        }
        public async Task<List<TEntity>> Update(List<TEntity> entities)
        {
            await _Context.SetAsModifiedAsync(entities);
            return entities;
        }
        public async Task<List<TEntity>> Delete(List<TEntity> entities)
        {
            await _Context.SetAsDeletedAsync(entities);
            return entities;
        }
        public async Task<List<TEntity>> Delete(Expression<Func<TEntity, bool>> BWhere)
        {
            var entities = await _DBSet.Where(BWhere).ToListAsync();
            await _Context.SetAsDeletedAsync(entities);
            return entities;
        }

        #endregion

        #region DB Query

        public async Task<List<TEntity>> ExecSP(string SP_Name, Dictionary<string, string> SP_Parameters)
        {
            string SqlParameters = "";
            foreach (var Par in SP_Parameters)
            {
                SqlParameters = SqlParameters + Par.Key + "='" + Par.Value + "'";
            }
            FormattableString Total_Query = $"SET TRANSACTION ISOLATION LEVEL SNAPSHOT; {SP_Name} {SqlParameters}";

            return await _Context.GetContext().Database.SqlQuery<TEntity>(Total_Query).ToListAsync();
        }

        public async Task<int> ExecQuery(string Query_String)
        {
            FormattableString Total_Query = $"SET TRANSACTION ISOLATION LEVEL SNAPSHOT; {Query_String}";
            return await Task.FromResult<int>(_Context.GetContext().Database.ExecuteSql(Total_Query));
        }

        public async Task<Object> DataQuery(string Query_String)
        {
            FormattableString Total_Query = $"SET TRANSACTION ISOLATION LEVEL SNAPSHOT; {Query_String}";
            return await _Context.GetContext().Database.SqlQuery<object>(Total_Query).ToListAsync();
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
