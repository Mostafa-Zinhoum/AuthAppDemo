using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Interfaces
{
    //
    // Summary:
    //     Defines a simple interface to map objects.
    public interface IObjectMapper
    {
        //
        // Summary:
        //     Converts an object to another. Creates a new object of TDestination.
        //
        // Parameters:
        //   source:
        //     Source object
        //
        // Type parameters:
        //   TDestination:
        //     Type of the destination object
        TDestination Map<TDestination>(object source);

        //
        // Summary:
        //     Execute a mapping from the source object to the existing destination object
        //
        // Parameters:
        //   source:
        //     Source object
        //
        //   destination:
        //     Destination object
        //
        // Type parameters:
        //   TSource:
        //     Source type
        //
        //   TDestination:
        //     Destination type
        //
        // Returns:
        //     Returns the same destination object after mapping operation
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        //
        // Summary:
        //     Project the input queryable.
        //
        // Parameters:
        //   source:
        //     Queryable source
        //
        // Type parameters:
        //   TDestination:
        //     Destination type
        //
        // Returns:
        //     Queryable result, use queryable extension methods to project and execute result
        //
        //
        // Remarks:
        //     Projections are only calculated once and cached
        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source);
    }
}
