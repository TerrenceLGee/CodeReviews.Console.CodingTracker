using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.SqlStatements;
using CodingTracker.TerrenceLGee.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CodingTracker.TerrenceLGee.Data.Repositories;

public class CoderRepository : ICoderRepository
{
    private readonly ConnectionString _connectionString;
    private readonly ILogger<CoderRepository> _logger;
    private string _errorMessage = string.Empty;

    public CoderRepository(
        ConnectionString connectionString,
        ILogger<CoderRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }


    public int AddCoder(Coder coder)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    FirstName = coder.FirstName,
                    LastName = coder.LastName
                };

                return connection.Execute(CoderStatements.InsertCoder, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(AddCoder)}\n" +
                            $"There was an error adding the coder {coder.FirstName} {coder.LastName}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(AddCoder)}\n" +
                            $"There was an unexpected error adding the coder {coder.FirstName} {coder.LastName}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }

    public Coder? GetCoder(string firstName, string lastName)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var coderLookup = new Dictionary<int, Coder>();
                var goalLookup = new Dictionary<int, CodingGoal>();
                var sessionLookup = new Dictionary<int, CodingSession>();
                var reportLookup = new Dictionary<int, CodingReport>();

                var parameters = new
                {
                    FirstName = firstName,
                    LastName = lastName
                };
                
                var coder = connection.Query<Coder?, CodingGoal?, CodingSession?, CodingReport?, Coder?>(
                    CoderStatements.GetCoder, (c, g, s, r) =>
                    {
                        if (c is null) return null;
                        if (!coderLookup.TryGetValue(c.Id, out var retrievedCoder))
                        {
                            coderLookup.Add(c.Id, retrievedCoder = c);
                        }

                        if (g is null) return retrievedCoder;
                        if (!goalLookup.TryGetValue(g.Id, out var codingGoal))
                        {
                            goalLookup.TryAdd(g.Id, codingGoal = g);
                            retrievedCoder.Goals.Add(codingGoal);
                        }
                        
                        if (s is null) return retrievedCoder;
                        if (!sessionLookup.TryGetValue(s.Id, out var codingSession)) 
                        {
                            sessionLookup.TryAdd(s.Id, codingSession = s);
                            codingGoal.Sessions.Add(codingSession);
                        }

                        if (r is null) return retrievedCoder;
                        if (!reportLookup.TryGetValue(r.Id, out var codingReport))
                        {
                            reportLookup.TryAdd(r.Id, codingReport = r);
                            retrievedCoder.Reports.Add(codingReport);
                        }
                        
                        return retrievedCoder;
                    }, parameters).FirstOrDefault();

                if (coder is not null && coder.Goals.Count != 0)
                {
                    coder.CurrentGoal = coder.Goals
                        .Where(g => !g.IsGoalFinished)
                        .Select(g => g)
                        .FirstOrDefault();
                }

                return coder;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(GetCoder)}\n" +
                            $"There was an error retrieving the coder {firstName} {lastName}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(GetCoder)}\n" +
                            $"There was an unexpected error retrieving the coder {firstName} {lastName}" +
                            $" from the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public bool CoderAlreadyExists(string firstName, string lastName)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                return connection.ExecuteScalar<int>(CoderStatements.GetCoderCount, parameters) == 1;
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(CoderAlreadyExists)}\n" +
                            $"There was an error checking the existence of the coder {firstName} {lastName}" +
                            $" int the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CoderRepository)}\nMethod: {nameof(CoderAlreadyExists)}\n" +
                            $"There was an unexpected error checking the existence the coder {firstName} {lastName}" +
                            $" in the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }
}