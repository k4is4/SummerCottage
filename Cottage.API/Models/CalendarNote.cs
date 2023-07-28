namespace Cottage.API.Models;

public partial class CalendarNote
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Note { get; set; } = null!;

    public int Color { get; set; }
}
