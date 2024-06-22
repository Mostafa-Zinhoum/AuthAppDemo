using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace AuthAppDemoService
{
    #region Application Interfaces

    public interface IService<TDto, TGetDto, TGetAllDto, TAddDto, TUpdateDto, TDeleteDto>
    {
        TDto Add(TAddDto dto);
        TUpdateDto Update(TUpdateDto dto);
        void Delete(TDeleteDto dto);
        TDto Get(TGetDto dto);
        ICollection<TGetDto> GetAll(TGetAllDto dto);
    }

    #endregion

    #region Application Services

    #region Application Services Interfaces
    /*
    //
    // Summary:
    //     This interface is intended to be used by ABP.
    public interface IAsyncQueryableExecuter
    {
        Task<int> CountAsync<T>(IQueryable<T> queryable);

        Task<List<T>> ToListAsync<T>(IQueryable<T> queryable);

        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable);

        Task<bool> AnyAsync<T>(IQueryable<T> queryable);
    }
    public class NullAsyncQueryableExecuter : IAsyncQueryableExecuter
    {
        public static NullAsyncQueryableExecuter Instance { get; } = new NullAsyncQueryableExecuter();

        public Task<int> CountAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.Count());
        }

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.ToList());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.Any());
        }
    }

    */



    #endregion
    


    #endregion

}
