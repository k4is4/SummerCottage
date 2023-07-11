using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cottage.API.Models;

public partial class Item
{
    [Key]
	public int? Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Status { get; set; }

    public string? Comment { get; set; }

    public int Category { get; set; }

    public virtual Category? CategoryNavigation { get; set; } = null!;
}
