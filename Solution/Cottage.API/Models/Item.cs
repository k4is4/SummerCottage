using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cottage.API.Models;

public partial class Item
{
    public int Id { get; set; }

	[StringLength(30, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 30 characters.")]
	public string Name { get; set; } = null!;

    public int Status { get; set; }

	[StringLength(100, ErrorMessage = "Comment should be 100 characters or less.")]
	public string? Comment { get; set; }

    public int Category { get; set; }

	public DateTime UpdatedOn { get; set; }
}
