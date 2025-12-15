namespace CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;

public class CreateCodingSessionDto
{
    public int GoalId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan? SessionDuration { get; set; }
    public string? Comments { get; set; }
    public bool IsSessionFinished { get; set; }
}