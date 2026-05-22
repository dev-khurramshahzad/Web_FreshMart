using System;
using System.Collections.Generic;

namespace FreshMart.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDetails { get; set; }

    public string? CategoryImage { get; set; }

    public string? CategoryStatus { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
