using System;
using System.Collections.Generic;

namespace AuthAppDemoDB.Models;

public partial class UserInfo
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public bool? IsPhoneNumberConfirmed { get; set; }

    public bool? IsEmailAddressConfirmed { get; set; }

    public string? ConfirmationCode { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public long? DeleteUserId { get; set; }

    public DateTime? DeleteTime { get; set; }

    public long? LastModificationUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? CreateUserId { get; set; }

    public DateTime? CreateTime { get; set; }
}
