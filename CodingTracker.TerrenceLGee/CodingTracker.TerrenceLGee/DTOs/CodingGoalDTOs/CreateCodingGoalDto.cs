namespace CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;

public class CreateCodingGoalDto
{
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
}