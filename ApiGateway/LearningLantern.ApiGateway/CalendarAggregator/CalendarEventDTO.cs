namespace LearningLantern.ApiGateway.CalendarAggregator;

public class CalendarEventDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
}