using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.SqlStatements;
using CodingTracker.TerrenceLGee.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CodingTracker.TerrenceLGee.Data.Repositories;

public class CodingGoalRepository : ICodingGoalRepository
{
    private readonly ConnectionString _connectionString;
    private readonly ILogger<CodingGoalRepository> _logger;
    private string _errorMessage = string.Empty;
    
    public CodingGoalRepository(
        ConnectionString connectionString, 
        ILogger<CodingGoalRepository> logger) 
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public int AddCodingGoal(CodingGoal goal)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    CoderId = goal.CoderId,
                    StartDate = goal.StartDate,
                    EndDate = goal.EndDate,
                    GoalHours = goal.GoalHours,
                    HoursNeededToReachGoal = goal.HoursNeededToReachGoal,
                    HoursCodedSoFar = goal.HoursCodedSoFar,
                    IsCurrentCodingGoal = goal.IsCurrentCodingGoal,
                    IsEndDateExpired = goal.IsEndDateExpired,
                    IsGoalMet = goal.IsGoalMet,
                    IsGoalFinished = goal.IsGoalFinished,
                    ActualEndDate = goal.ActualEndDate
                };

                return connection.Execute(CodingGoalStatements.InsertCodingGoal, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"Class: {nameof(CodingGoalRepository)}\nMethod: {nameof(AddCodingGoal)}\n" +
                            $"There was an error adding the coding goal for user {goal.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"Class: {nameof(CodingGoalRepository)}\nMethod: {nameof(AddCodingGoal)}\n" +
                            $"There was an unexpected error adding the coding goal for user {goal.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public int UpdateCodingGoal(CodingGoal goal)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    StartDate = goal.StartDate,
                    EndDate = goal.EndDate,
                    GoalHours = goal.GoalHours,
                    HoursNeededToReachGoal = goal.HoursNeededToReachGoal,
                    HoursCodedSoFar = goal.HoursCodedSoFar,
                    IsCurrentCodingGoal = goal.IsCurrentCodingGoal,
                    IsEndDateExpired = goal.IsEndDateExpired,
                    IsGoalMet = goal.IsGoalMet,
                    IsGoalFinished = goal.IsGoalFinished,
                    ActualEndDate = goal.ActualEndDate,
                    CoderId = goal.CoderId,
                    Id = goal.Id
                };

                return connection.Execute(CodingGoalStatements.UpdateCodingGoal, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"Class: {nameof(CodingGoalRepository)}\nMethod: {nameof(UpdateCodingGoal)}\n" +
                            $"There was an error updating the coding goal for user {goal.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"Class: {nameof(CodingGoalRepository)}\nMethod: {nameof(UpdateCodingGoal)}\n" +
                            $"There was an unexpected error updating the coding goal for user {goal.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public bool HasCodingGoals(int coderId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new { CoderId = coderId };

                return connection.ExecuteScalar<int>(CodingGoalStatements.CoderHasAnyGoals, parameters) > 0;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(HasCodingGoals)}\n" +
                            $"There was an error determining if there are any coding goals for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(HasCodingGoals)}\n" +
                            $"There was an unexpected error determining if there are any coding goals for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public CodingGoal? GetCurrentCodingGoal(int coderId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var goalLookup = new Dictionary<int, CodingGoal>();
                var sessionLookup = new Dictionary<int, CodingSession>();

                var parameters = new { CoderId = coderId };

                var goal = connection.Query<CodingGoal?, CodingSession?, CodingGoal?>(
                    CodingGoalStatements.GetCurrentCodingGoal, (g, s) =>
                    {
                        if (g is null) return null;
                        if (!goalLookup.TryGetValue(g.Id, out var currentGoal))
                        {
                            goalLookup.TryAdd(g.Id, currentGoal = g);
                        }

                        if (s is null) return currentGoal;
                        if (!sessionLookup.TryGetValue(s.Id, out var codingSession))
                        {
                            sessionLookup.TryAdd(s.Id, codingSession = s);
                            currentGoal.Sessions.Add(codingSession);
                        }

                        return currentGoal;
                    }, parameters).FirstOrDefault();

                return goal;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCurrentCodingGoal)}\n" +
                            $"There was an error retrieving the current coding goal for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCurrentCodingGoal)}\n" +
                            $"There was an unexpected error retrieving the current coding goal for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public CodingGoal? GetCodingGoal(int coderId, int goalId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var goalLookup = new Dictionary<int, CodingGoal>();
                var sessionLookup = new Dictionary<int, CodingSession>();
                
                var parameters = new { Id = goalId, CoderId = coderId };

                var goal = connection.Query<CodingGoal?, CodingSession?, CodingGoal?>(
                    CodingGoalStatements.GetCodingGoal, (g, s) =>
                    {
                        if (g is null) return null;
                        if (!goalLookup.TryGetValue(g.Id, out var goal))
                        {
                            goalLookup.TryAdd(g.Id, goal = g);
                        }

                        if (s is null) return goal;
                        if (!sessionLookup.TryGetValue(s.Id, out var codingSession))
                        {
                            sessionLookup.TryAdd(s.Id, codingSession = s);
                            goal.Sessions.Add(codingSession);
                        }

                        return goal;
                    }, parameters).FirstOrDefault();

                return goal;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCurrentCodingGoal)}\n" +
                            $"There was an error retrieving coding goal {goalId} for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCurrentCodingGoal)}\n" +
                            $"There was an unexpected error retrieving coding goal {goalId} for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public List<CodingGoal> GetCodingGoals(int coderId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var goalLookup = new Dictionary<int, CodingGoal>();
                var sessionLookup = new Dictionary<int, CodingSession>();

                var parameters = new { CoderId = coderId };

                var goals = connection.Query<CodingGoal?, CodingSession?, CodingGoal>(
                    CodingGoalStatements.GetCodingGoals, (g, s) =>
                    {
                        if (g is null) return null!;
                        if (!goalLookup.TryGetValue(g.Id, out var goal))
                        {
                            goalLookup.TryAdd(g.Id, goal = g);
                        }

                        if (s is null) return goal;
                        if (!sessionLookup.TryGetValue(s.Id, out var codingSession))
                        {
                            sessionLookup.TryAdd(s.Id, codingSession = s);
                            goal.Sessions.Add(codingSession);
                        }

                        return goal;
                    }, parameters).ToList();
                return goals;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCodingGoals)}\n" +
                            $"There was an error retrieving the coding goals for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingGoalRepository)}\nMethod: {nameof(GetCodingGoals)}\n" +
                            $"There was an unexpected error retrieving the coding goals for coder {coderId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}