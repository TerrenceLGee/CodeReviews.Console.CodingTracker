namespace CodingTracker.TerrenceLGee.Data.SqlStatements;

public static class TableCreationStatements
{
    public static string CreateCodersTable => @"
        CREATE TABLE IF NOT EXISTS coders(
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        FirstName TEXT,
        LastName TEXT);";

    public static string CreateCodingGoalsTable => @"
        CREATE TABLE IF NOT EXISTS goals(
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        CoderId INTEGER, 
        StartDate TEXT,
        EndDate TEXT,
        GoalHours INTEGER,
        HoursNeededToReachGoal INTEGER,
        HoursCodedSoFar INTEGER,
        IsCurrentCodingGoal INTEGER,
        IsEndDateExpired INTEGER,
        IsGoalMet INTEGER,
        IsGoalFinished INTEGER,
        ActualEndDate TEXT,
        FOREIGN KEY(CoderId) REFERENCES coders(id)
        ON DELETE CASCADE);";

    public static string CreateCodingSessionsTable => @"
        CREATE TABLE IF NOT EXISTS sessions(
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        GoalId INTEGER,
        StartTime TEXT,
        EndTime TEXT,
        SessionDuration TEXT,
        Comments TEXT,
        IsSessionFinished INTEGER,
        FOREIGN KEY(GoalId) REFERENCES goals(id)
        ON DELETE CASCADE);";

    public static string CreateCodingReportTable => @"
        CREATE TABLE IF NOT EXISTS reports(
         Id INTEGER PRIMARY KEY AUTOINCREMENT,
         CoderId INTEGER,
         TargetDaysSetForGoal INTEGER,
         TotalDaysSetForGoal INTEGER,
         TotalHoursWantingToCode INTEGER,
         TotalHoursNeededToCodeToReachGoal INTEGER,
         TotalHoursActuallyCoded INTEGER,
         HowManyEndDateExpired INTEGER,
         HowManyGoalMet INTEGER,
         TotalSessionsDuration INTEGER,
         NumberOfFinishedSessions INTEGER,
         ReportGenerated TEXT,
         TotalGoals INTEGER,
         TotalSessions INTEGER,
         FOREIGN KEY(CoderId) REFERENCES coders(id)
         ON DELETE CASCADE);";
}