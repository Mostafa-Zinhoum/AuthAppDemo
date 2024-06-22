using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Dtos
{
    //
    // Summary:
    //     This interface is defined to standardize to request a limited result.
    public interface ILimitedResultRequest
    {
        //
        // Summary:
        //     Max expected result count.
        int MaxResultCount { get; set; }
    }

    //
    // Summary:
    //     This interface is defined to standardize to request a paged result.
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        //
        // Summary:
        //     Skip count (beginning of the page).
        int SkipCount { get; set; }
    }

    //
    // Summary:
    //     This interface is defined to standardize to request a sorted result.
    public interface ISortedResultRequest
    {
        //
        // Summary:
        //     Sorting information. Should include sorting field and optionally a direction
        //     (ASC or DESC) Can contain more than one field separated by comma (,).
        string Sorting { get; set; }
    }
    //
    // Summary:
    //     This interface is defined to standardize to request a paged and sorted result.
    public interface IPagedAndSortedResultRequest : IPagedResultRequest, ILimitedResultRequest, ISortedResultRequest
    {
    }

    //
    // Summary:
    //     Defines common properties for entity based DTOs.
    //
    // Type parameters:
    //   TPrimaryKey:
    public interface IEntityDto<TPrimaryKey>
    {
        //
        // Summary:
        //     Id of the entity.
        TPrimaryKey Id { get; set; }
    }
    //
    // Summary:
    //     A shortcut of Abp.Application.Services.Dto.IEntityDto`1 for most used primary
    //     key type (System.Int32).
    public interface IEntityDto : IEntityDto<int>
    {
    }

    //
    // Summary:
    //     This interface is defined to standardize to return a page of items to clients.
    //
    //
    // Type parameters:
    //   T:
    //     Type of the items in the Abp.Application.Services.Dto.IListResult`1.Items list
    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
    {
    }
    //
    // Summary:
    //     This interface is defined to standardize to return a list of items to clients.
    //
    //
    // Type parameters:
    //   T:
    //     Type of the items in the Abp.Application.Services.Dto.IListResult`1.Items list
    public interface IListResult<T>
    {
        //
        // Summary:
        //     List of items.
        IReadOnlyList<T> Items { get; set; }
    }

    //
    // Summary:
    //     This interface is defined to standardize to set "Total Count of Items" to a DTO.
    public interface IHasTotalCount
    {
        //
        // Summary:
        //     Total count of Items.
        int TotalCount { get; set; }
    }

}
