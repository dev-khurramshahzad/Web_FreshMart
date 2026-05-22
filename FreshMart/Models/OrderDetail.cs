using System;
using System.Collections.Generic;

namespace FreshMart.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderFid { get; set; }

    public int? ItemFid { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Item? ItemF { get; set; }
}
