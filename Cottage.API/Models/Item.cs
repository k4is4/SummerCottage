using System;
using System.Collections.Generic;

namespace Cottage.API.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public string? Comment { get; set; }

    public int Category { get; set; }

	public DateTime UpdatedOn { get; set; }
}
