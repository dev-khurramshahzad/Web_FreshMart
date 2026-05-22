using System;
using System.Collections.Generic;

namespace FreshMart.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string? Name { get; set; }

    public int? CategoryFid { get; set; }

    public decimal? Sprice { get; set; }

    public decimal? Pprice { get; set; }

    public string? StockUnit { get; set; }

    public double? AvailableStock { get; set; }

    public string? Status { get; set; }

    public DateOnly? ExpDate { get; set; }

    public int? Rating { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual Category? CategoryF { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
