using AuthAppDemoService.Basics.Dtos;
using AuthAppDemoService.Basics.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;
using AuthAppDemoDBInfra;


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
    public abstract class CrudAppServiceBase<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput> : IApplicationService //,ApplicationService
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Reference to the logger to write logs.
        /// </summary>
        public readonly ILogger Logger;

        /// <summary>
        /// Reference to the object to object mapper.
        /// </summary>
        public readonly IObjectMapper ObjectMapper;

        /// <summary>
        /// Reference to <see cref="IUnitOfWork"/>.
        /// </summary>
        public readonly IUnitOfWork UnitOfWork;

        protected CrudAppServiceBase(IUnitOfWork unitOfWork,IObjectMapper objectMapper)
        {
            UnitOfWork = unitOfWork;
            ObjectMapper = objectMapper;    
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        ///
        /*
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
        */
        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        /// 
        /*
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
        */
        /// <summary>
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
        /// It should filter query if needed, but should not do sorting or paging.
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
        /// methods.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual Expression<Func<TEntity, bool>> CreateFilteredQuery(TGetAllInput input)
        {
            //return UnitOfWork.Repository<TEntity>().GetAll();
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression expression = Expression.Constant(true); // Start with 'true' for dynamic composition

            foreach (var property in typeof(TGetAllInput).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.Name == "OrderType" || property.Name == "PageIndex" || property.Name == "PageCount")
                {
                    continue; // Skip these properties
                }

                var entityProperty = typeof(TEntity).GetProperty(property.Name);
                if (entityProperty == null) continue; // Skip if no matching property

                var propertyValue = property.GetValue(null);//property.GetValue(filter, null);
                if (propertyValue == null) continue; // Skip null values

                var leftSide = Expression.Property(parameter, entityProperty.Name);
                var rightSide = Expression.Constant(propertyValue);

                var equalExpression = Expression.Equal(leftSide, rightSide);
                expression = Expression.AndAlso(expression, equalExpression);
            }

            var finalExpression = Expression.Lambda<Func<TEntity, bool>>(expression, parameter);
            return finalExpression;
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
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
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
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
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
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
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
        protected AsyncCrudAppService(IUnitOfWork unitOfWork, IObjectMapper objectMapper)
            : base(unitOfWork, objectMapper)
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

            //long totalCount = await UnitOfWork.Repository<TEntity>().Count(); //AsyncQueryableExecuter.CountAsync(query);
            var result = await UnitOfWork.Repository<TEntity>().GetPagination(query, KeyOrder: null ,orderBy: Enum_DB_Order_By.Ascending,0,0);

            return new PagedResultDto<TEntityDto>(
                result.Total_Count ,
                result.Data as IReadOnlyList<TEntityDto>
            );
        }

        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            //CheckCreatePermission();

            var entity = MapToEntity(input);

            entity = await UnitOfWork.Repository<TEntity>().Insert(entity);
            UnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            //CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);
            UnitOfWork.SaveChanges();

            return MapToEntityDto(entity);
        }

        public virtual async Task DeleteAsync(TDeleteInput input)
        {
            //CheckDeletePermission();

            await UnitOfWork.Repository<TEntity>().Delete(x=>x.Id.Equals(input.Id));
        }

        protected virtual async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {   
             return await UnitOfWork.Repository<TEntity>().GetSingle(x=>x.Id.Equals(id));
        }
    }

}
