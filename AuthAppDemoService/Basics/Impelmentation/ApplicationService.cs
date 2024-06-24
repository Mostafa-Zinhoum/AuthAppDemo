using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Basics.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Impelmentation
{

    public abstract class ApplicationService : IApplicationService
    {
        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }
        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    throw new Exception("Must set UnitOfWorkManager before use it.");
                }

                return _unitOfWork;
            }
            set { _unitOfWork = value; }
        }
        private IUnitOfWork _unitOfWork;
    }

    /// <summary>
    /// This is a common base class for CrudAppService and AsyncCrudAppService classes.
    /// Inherit either from CrudAppService or AsyncCrudAppService, not from this class.
    /// </summary>
    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : ApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        //protected readonly IRepository<TEntity> Repository;

        //protected CrudAppServiceBase(IRepository<TEntity> repository)
        //{
        //    Repository = repository;
        //}

        protected CrudAppServiceBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
        /// It should filter query if needed, but should not do sorting or paging.
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
        /// methods.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            return Repository.GetAll();
        }

        /// <summary>
        /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TEntityDto"/>.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// Maps <typeparamref name="TEntityDto"/> to <typeparamref name="TEntity"/> to create a new entity.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
            return ObjectMapper.Map<TEntity>(createInput);
        }

        /// <summary>
        /// Maps <typeparamref name="TUpdateInput"/> to <typeparamref name="TEntity"/> to update the entity.
        /// It uses <see cref="IObjectMapper"/> by default.
        /// It can be overrided for custom mapping.
        /// </summary>
        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto>
        : AsyncCrudAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected AsyncCrudAppService(IRepository<TEntity, int> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedAndSortedResultRequest
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
       where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
           where TGetInput : IEntityDto<TPrimaryKey>
           where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        

        //protected AsyncCrudAppService(IRepository<TEntity> repository)
        //    : base(repository)
        //{
        //    AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        //}
        protected AsyncCrudAppService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }

        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            //CheckGetPermission();

            var entity = await GetEntityByIdAsync(input.Id);
            return MapToEntityDto(entity);
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            //CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await UnitOfWork.Repository<TEntity>().Count(); //AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            //CheckCreatePermission();

            var entity = MapToEntity(input);

            await Repository.InsertAsync(entity);
            await UnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            //CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);
            await UnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        public virtual async Task DeleteAsync(TDeleteInput input)
        {
            //CheckDeletePermission();

            return await UnitOfWork.Repository<TEntity>().DeleteAsync(input.Id);
        }

        protected virtual Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            return await Repository.GetAsync(id);
        }
    }

}
