using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Dtos
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        public static int DefaultMaxResultCount { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
    }


    // <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }

    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedAndSortedResultRequestDto : PagedResultRequestDto, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }

    /// <summary>
    /// A shortcut of <see cref="EntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    [Serializable]
    public class EntityDto : EntityDto<int>, IEntityDto
    {
        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(int id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Implements common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key</typeparam>
    [Serializable]
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {
        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        public PagedResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }
    }

    /// <summary>
    /// Implements <see cref="IListResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="Items"/> list</typeparam>
    [Serializable]
    public class ListResultDto<T> : IListResult<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        public ListResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}
