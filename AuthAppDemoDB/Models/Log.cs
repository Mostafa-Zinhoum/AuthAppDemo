using System;
using System.Collections.Generic;

namespace AuthAppDemoDB.Models;

public partial class Log
{
    public int Id { get; set; }

    public string? LogLevel { get; set; }

    public string? EventId { get; set; }

    public string? EventName { get; set; }

    public string? Message { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? ExceptionStackTrace { get; set; }

    public string? ExceptionSource { get; set; }

    public DateTime? Created { get; set; }
}
