using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public virtual User User { get; set; } = null!;
}
