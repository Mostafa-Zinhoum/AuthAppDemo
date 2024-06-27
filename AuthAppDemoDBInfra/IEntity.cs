using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoDBInfra
{
    //
    // Summary:
    //     Defines interface for base entity type. All entities in the system must implement
    //     this interface.
    //
    // Type parameters:
    //   TPrimaryKey:
    //     Type of the primary key of the entity
    public interface IEntity<TPrimaryKey>
    {
        //
        // Summary:
        //     Unique identifier for this entity.
        TPrimaryKey Id { get; set; }

    }
}
