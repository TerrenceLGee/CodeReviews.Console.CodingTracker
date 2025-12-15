using CodingTracker.TerrenceLGee.Data.Handlers;
using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.SqlStatements;
using CodingTracker.TerrenceLGee.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CodingTracker.TerrenceLGee.Data.Repositories;

public class CodingSessionRepository : ICodingSessionRepository
{
    private readonly ConnectionString _connectionString;
    private readonly ILogger<CodingSessionRepository> _logger;
    private string _errorMessage = string.Empty;

    public CodingSessionRepository(
        ConnectionString connectionString,
        ILogger<CodingSessionRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
        SqlMapper.AddTypeHandler(new TimeSpanHandler());
    }


    public int AddCodingSession(CodingSession session)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    GoalId = session.GoalId,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    SessionDuration = session.SessionDuration,
                    Comments = session.Comments,
                    IsSessionFinished = session.IsSessionFinished
                };

                return connection.Execute(CodingSessionStatements.InsertCodingSession, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(AddCodingSession)}\n" +
                            $"There was an error adding a coding session for coding goal" +
                            $" {session.GoalId}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(AddCodingSession)}\n" +
                            $"There was an unexpected error adding a coding session for coding goal" +
                            $" {session.GoalId}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public int UpdateCodingSession(CodingSession session)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    Id = session.Id,
                    GoalId = session.GoalId,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    SessionDuration = session.SessionDuration,
                    Comments = session.Comments,
                    IsSessionFinished = session.IsSessionFinished
                };

                return connection.Execute(CodingSessionStatements.UpdateCodingSession, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(UpdateCodingSession)}\n" +
                            $"There was an error updating coding session {session.Id} for coding goal" +
                            $" {session.GoalId}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(UpdateCodingSession)}\n" +
                            $"There was an error updating coding session {session.Id} for coding goal" +
                            $" {session.GoalId}: {ex.Message}\n\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public int DeleteCodingSession(int goalId, int sessionId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    GoalId = goalId,
                    Id = sessionId
                };

                return connection.Execute(CodingSessionStatements.DeleteCodingSession, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(DeleteCodingSession)}\n" +
                            $"There was an error deleting coding session {sessionId} for coding goal" +
                            $" {goalId}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(DeleteCodingSession)}\n" +
                            $"There was an error deleting coding session {sessionId} for coding goal" +
                            $" {goalId}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public CodingSession? GetCodingSession(int goalId, int sessionId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    Id = sessionId,
                    GoalId = goalId
                };

                return connection
                    .QuerySingleOrDefault<CodingSession>(CodingSessionStatements.GetCodingSession, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(GetCodingSession)}\n" +
                            $"There was an error retrieving the coding session {sessionId} for coding goal {goalId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(GetCodingSession)}\n" +
                            $"There was an unexpected error retrieving the coding session {sessionId} for coding goal {goalId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public List<CodingSession> GetCodingSessions(int goalId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new { GoalId = goalId };

                return connection
                    .Query<CodingSession>(CodingSessionStatements.GetCodingSessions, parameters)
                    .ToList();
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(GetCodingSessions)}\n" +
                            $"There was an error retrieving the coding sessions for coding goal {goalId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingSessionRepository)}\nMethod: {nameof(GetCodingSessions)}\n" +
                            $"There was an unexpected error retrieving the coding sessions for coding goal {goalId}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}