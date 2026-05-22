using System;
using System.Collections.Generic;

namespace FreshMart.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerFid { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Customer? CustomerF { get; set; }
}
