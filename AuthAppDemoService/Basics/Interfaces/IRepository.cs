﻿using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Basics.Impelmentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{

    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {


        #region Get And Search Filters Operations


        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> BWhere);
        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> BWhere, Expression<Func<TEntity, Int64>> KeyOrder, Enum_DB_Order_By orderBy = Enum_DB_Order_By.Ascending, int Page_Index = 0, int Page_Count = 0);
        Task<Pagination_Result> GetPagination(int Page_Index = 0, int Page_Count = 0);
        Task<Pagination_Result> GetPagination(Expression<Func<TEntity, bool>> BWhere, Expression<Func<TEntity, Int64>> KeyOrder, Enum_DB_Order_By orderBy = Enum_DB_Order_By.Ascending, int Page_Index = 0, int Page_Count = 0);
        Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> BWhere);
        Task<TEntity> FirstOrDefault();
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> BWhere);
        Task<TEntity> LastOrDefault(Expression<Func<TEntity, dynamic>> keySelector);
        Task<TEntity> LastOrDefault(Expression<Func<TEntity, dynamic>> keySelector, Expression<Func<TEntity, bool>> BWhere);


        #endregion

        #region Numberic Operations

        Task<Int64> Max(Expression<Func<TEntity, Int64>> KeyMax);
        Task<Int64> Max(Expression<Func<TEntity, Int64>> KeyMax, Expression<Func<TEntity, bool>> BWhere);
        Task<Int64> Count();
        Task<Int64> Count(Expression<Func<TEntity, bool>> BWhere);

        #endregion

        #region Row Operations

        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(TEntity entity);

        #endregion

        #region Rows List Operations

        Task<List<TEntity>> Insert(List<TEntity> entities);
        Task<List<TEntity>> Update(List<TEntity> entities);
        Task<List<TEntity>> Delete(List<TEntity> entities);
        Task<List<TEntity>> Delete(Expression<Func<TEntity, bool>> BWhere);

        #endregion

        #region DB Query

        Task<List<TEntity>> ExecSP(string SP_Name, Dictionary<string, string> SP_Parameters);
        Task<int> ExecQuery(string Query_String);
        Task<Object> DataQuery(string Query_String);

        #endregion


    }
    /*
    //
    // Summary:
    //     This interface must be implemented by all repositories to identify them by convention.
    //     Implement generic version instead of this one.
    //public interface IRepository
    //{
    //}


    //
    // Summary:
    //     This interface is implemented by all repositories to ensure implementation of
    //     fixed methods.
    //
    // Type parameters:
    //   TEntity:
    //     Main Entity type this repository works on
    //
    //   TPrimaryKey:
    //     Primary key type of the entity
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>//IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAll();

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve readonly entities from entire
        //     table.
        //
        // Returns:
        //     Readonly IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAllReadonly();

        //
        // Summary:
        //     Used to get async IQueryable that is used to retrieve readonly entities from
        //     entire table.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        Task<IQueryable<TEntity>> GetAllReadonlyAsync();

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        Task<IQueryable<TEntity>> GetAllAsync();

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //     One or more
        //
        // Parameters:
        //   propertySelectors:
        //     A list of include expressions.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve entities from entire table.
        //     One or more
        //
        // Parameters:
        //   propertySelectors:
        //     A list of include expressions.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        //
        // Summary:
        //     Used to get a IQueryable that is used to retrieve readonly entities from entire
        //     table.
        //
        // Parameters:
        //   propertySelectors:
        //     A list of include expressions.
        //
        // Returns:
        //     Readonly IQueryable to be used to select entities from database
        IQueryable<TEntity> GetAllReadonlyIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        //
        // Summary:
        //     Used to get async IQueryable that is used to retrieve entities from entire table.
        //     One or more
        //
        // Parameters:
        //   propertySelectors:
        //     A list of include expressions.
        //
        // Returns:
        //     IQueryable to be used to select entities from database
        Task<IQueryable<TEntity>> GetAllReadonlyIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        //
        // Summary:
        //     Used to get all entities.
        //
        // Returns:
        //     List of all entities
        List<TEntity> GetAllList();

        //
        // Summary:
        //     Used to get all entities.
        //
        // Returns:
        //     List of all entities
        Task<List<TEntity>> GetAllListAsync();

        //
        // Summary:
        //     Used to get all entities based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        //
        // Returns:
        //     List of all entities
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Used to get all entities based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        //
        // Returns:
        //     List of all entities
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Used to run a query over entire entities. Abp.Domain.Uow.UnitOfWorkAttribute
        //     attribute is not always necessary (as opposite to Abp.Domain.Repositories.IRepository`2.GetAll)
        //     if queryMethod finishes IQueryable with ToList, FirstOrDefault etc..
        //
        // Parameters:
        //   queryMethod:
        //     This method is used to query over entities
        //
        // Type parameters:
        //   T:
        //     Type of return value of this method
        //
        // Returns:
        //     Query result
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        //
        // Summary:
        //     Gets an entity with given primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity
        TEntity Get(TPrimaryKey id);

        //
        // Summary:
        //     Gets an entity with given primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity
        Task<TEntity> GetAsync(TPrimaryKey id);

        //
        // Summary:
        //     Gets exactly one entity with given predicate. Throws exception if no entity or
        //     more than one entity.
        //
        // Parameters:
        //   predicate:
        //     Entity
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets exactly one entity with given predicate. Throws exception if no entity or
        //     more than one entity.
        //
        // Parameters:
        //   predicate:
        //     Entity
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets an entity with given primary key or null if not found.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity or null
        TEntity FirstOrDefault(TPrimaryKey id);

        //
        // Summary:
        //     Gets an entity with given primary key or null if not found.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to get
        //
        // Returns:
        //     Entity or null
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        //
        // Summary:
        //     Gets an entity with given given predicate or null if not found.
        //
        // Parameters:
        //   predicate:
        //     Predicate to filter entities
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets an entity with given given predicate or null if not found.
        //
        // Parameters:
        //   predicate:
        //     Predicate to filter entities
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Creates an entity with given primary key without database access.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity to load
        //
        // Returns:
        //     Entity
        TEntity Load(TPrimaryKey id);

        //
        // Summary:
        //     Inserts a new entity.
        //
        // Parameters:
        //   entity:
        //     Inserted entity
        TEntity Insert(TEntity entity);

        //
        // Summary:
        //     Inserts a new entity.
        //
        // Parameters:
        //   entity:
        //     Inserted entity
        Task<TEntity> InsertAsync(TEntity entity);

        //
        // Summary:
        //     Inserts a new entity and gets it's Id. It may require to save current unit of
        //     work to be able to retrieve id.
        //
        // Parameters:
        //   entity:
        //     Entity
        //
        // Returns:
        //     Id of the entity
        TPrimaryKey InsertAndGetId(TEntity entity);

        //
        // Summary:
        //     Inserts a new entity and gets it's Id. It may require to save current unit of
        //     work to be able to retrieve id.
        //
        // Parameters:
        //   entity:
        //     Entity
        //
        // Returns:
        //     Id of the entity
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        //
        // Summary:
        //     Inserts or updates given entity depending on Id's value.
        //
        // Parameters:
        //   entity:
        //     Entity
        TEntity InsertOrUpdate(TEntity entity);

        //
        // Summary:
        //     Inserts or updates given entity depending on Id's value.
        //
        // Parameters:
        //   entity:
        //     Entity
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        //
        // Summary:
        //     Inserts or updates given entity depending on Id's value. Also returns Id of the
        //     entity. It may require to save current unit of work to be able to retrieve id.
        //
        //
        // Parameters:
        //   entity:
        //     Entity
        //
        // Returns:
        //     Id of the entity
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        //
        // Summary:
        //     Inserts or updates given entity depending on Id's value. Also returns Id of the
        //     entity. It may require to save current unit of work to be able to retrieve id.
        //
        //
        // Parameters:
        //   entity:
        //     Entity
        //
        // Returns:
        //     Id of the entity
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        //
        // Summary:
        //     Updates an existing entity.
        //
        // Parameters:
        //   entity:
        //     Entity
        TEntity Update(TEntity entity);

        //
        // Summary:
        //     Updates an existing entity.
        //
        // Parameters:
        //   entity:
        //     Entity
        Task<TEntity> UpdateAsync(TEntity entity);

        //
        // Summary:
        //     Updates an existing entity.
        //
        // Parameters:
        //   id:
        //     Id of the entity
        //
        //   updateAction:
        //     Action that can be used to change values of the entity
        //
        // Returns:
        //     Updated entity
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        //
        // Summary:
        //     Updates an existing entity.
        //
        // Parameters:
        //   id:
        //     Id of the entity
        //
        //   updateAction:
        //     Action that can be used to change values of the entity
        //
        // Returns:
        //     Updated entity
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        //
        // Summary:
        //     Deletes an entity.
        //
        // Parameters:
        //   entity:
        //     Entity to be deleted
        void Delete(TEntity entity);

        //
        // Summary:
        //     Deletes an entity.
        //
        // Parameters:
        //   entity:
        //     Entity to be deleted
        Task DeleteAsync(TEntity entity);

        //
        // Summary:
        //     Deletes an entity by primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity
        void Delete(TPrimaryKey id);

        //
        // Summary:
        //     Deletes an entity by primary key.
        //
        // Parameters:
        //   id:
        //     Primary key of the entity
        Task DeleteAsync(TPrimaryKey id);

        //
        // Summary:
        //     Deletes many entities by function. Notice that: All entities fits to given predicate
        //     are retrieved and deleted. This may cause major performance problems if there
        //     are too many entities with given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        void Delete(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Deletes many entities by function. Notice that: All entities fits to given predicate
        //     are retrieved and deleted. This may cause major performance problems if there
        //     are too many entities with given predicate.
        //
        // Parameters:
        //   predicate:
        //     A condition to filter entities
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository.
        //
        // Returns:
        //     Count of entities
        int Count();

        //
        // Summary:
        //     Gets count of all entities in this repository.
        //
        // Returns:
        //     Count of entities
        Task<int> CountAsync();

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        int Count(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate.
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository (use if expected return value is
        //     greater than System.Int32.MaxValue.
        //
        // Returns:
        //     Count of entities
        long LongCount();

        //
        // Summary:
        //     Gets count of all entities in this repository (use if expected return value is
        //     greater than System.Int32.MaxValue.
        //
        // Returns:
        //     Count of entities
        Task<long> LongCountAsync();

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate (use this
        //     overload if expected return value is greater than System.Int32.MaxValue).
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        //
        // Summary:
        //     Gets count of all entities in this repository based on given predicate (use this
        //     overload if expected return value is greater than System.Int32.MaxValue).
        //
        // Parameters:
        //   predicate:
        //     A method to filter count
        //
        // Returns:
        //     Count of entities
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);
    }
    */
}
