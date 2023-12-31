﻿using System.ComponentModel.DataAnnotations;

namespace Cottage.API.Models;

public partial class CalendarEvent
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

	[StringLength(200, ErrorMessage = "Note should be 200 characters or less.")]
	public string Note { get; set; } = null!;

    public int Color { get; set; }

	public DateTime UpdatedOn { get; set; }
}
