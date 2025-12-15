using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.SqlStatements;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CodingTracker.TerrenceLGee.Data;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ConnectionString _connectionString;
    private readonly ILogger<DatabaseInitializer> _logger;
    private string _errorMessage = string.Empty;

    public DatabaseInitializer(
        ConnectionString connectionString,
        ILogger<DatabaseInitializer> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }


    public void InitializeDatabase()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                connection.Execute(TableCreationStatements.CreateCodersTable);

                connection.Execute(TableCreationStatements.CreateCodingGoalsTable);

                connection.Execute(TableCreationStatements.CreateCodingSessionsTable);

                connection.Execute(TableCreationStatements.CreateCodingReportTable);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(DatabaseInitializer)}\nMethod: {nameof(InitializeDatabase)}\n" + 
                            $"There was an error initializing the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            throw;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(DatabaseInitializer)}\nMethod: {nameof(InitializeDatabase)}\n" + 
                            $"There was an unexpected error initializing the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            throw;
        }
    }
}