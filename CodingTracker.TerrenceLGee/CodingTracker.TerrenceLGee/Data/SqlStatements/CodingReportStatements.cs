namespace CodingTracker.TerrenceLGee.Data.SqlStatements;

public static class CodingReportStatements
{
    public static string InsertCodingReport => @"
        INSERT INTO reports(CoderId, TargetDaysSetForGoal, TotalDaysSetForGoal, TotalHoursWantingToCode, 
        TotalHoursNeededToCodeToReachGoal, TotalHoursActuallyCoded, HowManyEndDateExpired, 
        HowManyGoalMet, TotalSessionsDuration, NumberOfFinishedSessions,
        ReportGenerated, TotalGoals, TotalSessions) 
        VALUES(@CoderId, @TargetDaysSetForGoal, @TotalDaysSetForGoal, @TotalHoursWantingToCode, 
        @TotalHoursNeededToCodeToReachGoal, @TotalHoursActuallyCoded, @HowManyEndDateExpired, 
        @HowManyGoalMet, @TotalSessionsDuration, @NumberOfFinishedSessions, 
        @ReportGenerated, @TotalGoals, @TotalSessions);";

    public static string GetCodingReport => @"
        SELECT * FROM reports WHERE Id = @Id AND CoderId = @CoderId;";

    public static string GetCodingReports => @"
        SELECT * FROM reports WHERE CoderId = @CoderId;";
}