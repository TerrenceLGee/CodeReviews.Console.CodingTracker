namespace CodingTracker.TerrenceLGee.Models;

public class CodingGoal
{
    public int Id { get; set; }
    public int CoderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int GoalHours { get; set; }
    public int HoursNeededToReachGoal { get; set; }
    public int HoursCodedSoFar { get; set; }
    public bool IsCurrentCodingGoal { get; set; }
    public bool IsEndDateExpired { get; set; }
    public bool IsGoalMet { get; set; }
    public bool IsGoalFinished { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public List<CodingSession> Sessions { get; set; } = [];
}