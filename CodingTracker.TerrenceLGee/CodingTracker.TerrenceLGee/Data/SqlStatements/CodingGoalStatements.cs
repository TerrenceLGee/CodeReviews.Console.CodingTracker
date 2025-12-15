namespace CodingTracker.TerrenceLGee.Data.SqlStatements;

public static class CodingGoalStatements
{
    public static string InsertCodingGoal => @"
        INSERT INTO goals(CoderId, StartDate, EndDate, GoalHours, 
        HoursNeededToReachGoal, HoursCodedSoFar, IsCurrentCodingGoal, IsEndDateExpired, IsGoalMet, IsGoalFinished, ActualEndDate) 
        VALUES(@CoderId, @StartDate, @EndDate, @GoalHours, @HoursNeededToReachGoal, @HoursCodedSoFar, @IsCurrentCodingGoal,
               @IsEndDateExpired, @IsGoalMet, @IsGoalFinished, @ActualEndDate);";

    public static string UpdateCodingGoal => @"
        UPDATE goals SET StartDate = @StartDate, EndDate = @EndDate, GoalHours = @GoalHours, 
        HoursNeededToReachGoal = @HoursNeededToReachGoal, HoursCodedSoFar = @HoursCodedSoFar, 
        IsCurrentCodingGoal = @IsCurrentCodingGoal, IsEndDateExpired = @IsEndDateExpired, 
        IsGoalMet = @IsGoalMet, IsGoalFinished = @IsGoalFinished, ActualEndDate = @ActualEndDate
        WHERE CoderId = @CoderId AND Id = @Id;";
    
    public static string GetCurrentCodingGoal => @"
        SELECT * FROM goals LEFT JOIN sessions ON goals.Id = sessions.GoalId 
        WHERE goals.CoderId = @CoderId AND goals.IsCurrentCodingGoal = 1;";

    public static string GetCodingGoal => @"
        SELECT * FROM goals LEFT JOIN sessions ON goals.Id = sessions.GoalId 
        WHERE goals.CoderId = @CoderId AND goals.Id = @Id;";

    public static string GetCodingGoals => @"
        SELECT * FROM goals LEFT JOIN sessions ON goals.Id = sessions.GoalId 
        WHERE goals.CoderId = @CoderId;";

    public static string CoderHasAnyGoals => @"
        SELECT COUNT(*) FROM goals WHERE CoderId = @CoderId;";
}