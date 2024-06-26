using System;
using System.Collections.Generic;

namespace AuthAppDemoDB.Models;

public partial class Error
{
    public long Id { get; set; }

    public string? Values { get; set; }

    public string? Created { get; set; }
}
