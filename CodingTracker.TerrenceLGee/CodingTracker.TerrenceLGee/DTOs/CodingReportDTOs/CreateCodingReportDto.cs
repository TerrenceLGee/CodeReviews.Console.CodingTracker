namespace CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;

public class CreateCodingReportDto
{
    public int CoderId { get; set; }
    public int TargetDaysSetForGoal { get; set; }
    public int TotalDaysSetForGoal { get; set; }
    public int TotalHoursWantingToCode { get; set; }
    public int TotalHoursNeededToCodeToReachGoal { get; set; }
    public int TotalHoursActuallyCoded { get; set; }
    public int HowManyEndDateExpired { get; set; }
    public int HowManyGoalMet { get; set; }
    public TimeSpan TotalSessionsDuration { get; set; }
    public int NumberOfFinishedSessions { get; set; }
    public DateTime ReportGenerated { get; set; }
    public int TotalGoals { get; set; }
    public int TotalSessions { get; set; }
}