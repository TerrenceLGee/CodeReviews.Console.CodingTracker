namespace CodingTracker.TerrenceLGee.Data.SqlStatements;

public static class CodingSessionStatements
{
    public static string InsertCodingSession => @"
        INSERT INTO sessions(GoalId, StartTime, EndTime, SessionDuration, Comments, IsSessionFinished) 
        VALUES(@GoalId, @StartTime, @EndTime, @SessionDuration, @Comments, @IsSessionFinished);";

    public static string UpdateCodingSession => @"
        UPDATE sessions SET StartTime = @StartTime, EndTime = @EndTime, SessionDuration = @SessionDuration, 
        Comments = @Comments, IsSessionFinished = @IsSessionFinished WHERE GoalId = @GoalId AND Id = @Id;";

    public static string DeleteCodingSession => @"
        DELETE FROM sessions WHERE GoalId = @GoalId AND Id = @Id;";

    public static string GetCodingSession => @"
        SELECT * FROM sessions WHERE Id = @Id AND GoalId = @GoalId;";

    public static string GetCodingSessions => @"
        SELECT * FROM sessions WHERE GoalId = @GoalId;";
}