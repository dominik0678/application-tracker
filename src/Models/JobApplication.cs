namespace ApplicationTracker.Models;

public class JobApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Company { get; set; } = "";
    public string Title { get; set; } = "";
    public string Location { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Now;
    public Status Status { get; set; } = Status.Interested;
    public string Contact { get; set; } = "";
    public string Link { get; set; } = "";
    public string Notes { get; set; } = "";
}