using System;
using System.Collections.Generic;

namespace FreshMart.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? CustomerFid { get; set; }

    public int? ItemFid { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public virtual Customer? CustomerF { get; set; }

    public virtual Item? ItemF { get; set; }
}
