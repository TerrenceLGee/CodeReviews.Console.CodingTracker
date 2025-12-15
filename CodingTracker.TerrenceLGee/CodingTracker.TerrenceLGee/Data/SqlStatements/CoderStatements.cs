namespace CodingTracker.TerrenceLGee.Data.SqlStatements;

public static class CoderStatements
{
    public static string InsertCoder => @"
        INSERT INTO coders(FirstName, LastName) 
        VALUES(@FirstName, @LastName);";

    public static string GetCoderCount => @"
        SELECT COUNT(*) FROM coders WHERE 
        FirstName LIKE @FirstName And LastName LIKE @LastName;";

    public static string GetCoder => @"
        SELECT * FROM coders LEFT JOIN goals ON coders.Id = goals.CoderId 
        LEFT JOIN sessions ON goals.Id = sessions.GoalId LEFT JOIN reports ON coders.Id = reports.CoderId 
        WHERE FirstName = @FirstName 
        AND LastName = @LastName;";
}