using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using AuthAppDemoDBInfra;

namespace AuthAppDemoDB.Models;

public partial class Item : IEntity<long>
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? CreateuserId { get; set; }

    public DateTime? CreateDate { get; set; }

    public long? UpdateUserId { get; set; }

    public DateTime? UpdateDate { get; set; }
}
