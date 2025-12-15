using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.Data.SqlStatements;
using CodingTracker.TerrenceLGee.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CodingTracker.TerrenceLGee.Data.Repositories;

public class CodingReportRepository : ICodingReportRepository
{
    private readonly ConnectionString _connectionString;
    private readonly ILogger<CodingReportRepository> _logger;
    private string _errorMessage = string.Empty;

    public CodingReportRepository(
        ConnectionString connectionString,
        ILogger<CodingReportRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }


    public int AddCodingReport(CodingReport report)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new
                {
                    CoderId = report.CoderId,
                    TargetDaysSetForGoal = report.TargetDaysSetForGoal,
                    TotalDaysSetForGoal = report.TotalDaysSetForGoal,
                    TotalHoursWantingToCode = report.TotalHoursWantingToCode,
                    TotalHoursNeededToCodeToReachGoal = report.TotalHoursNeededToCodeToReachGoal,
                    TotalHoursActuallyCoded = report.TotalHoursActuallyCoded,
                    HowManyEndDateExpired = report.HowManyEndDateExpired,
                    HowManyGoalMet = report.HowManyGoalMet,
                    TotalSessionsDuration = report.TotalSessionsDuration,
                    NumberOfFinishedSessions = report.NumberOfFinishedSessions,
                    ReportGenerated = report.ReportGenerated,
                    TotalGoals = report.TotalGoals,
                    TotalSessions = report.TotalSessions
                };

                return connection.Execute(CodingReportStatements.InsertCodingReport, parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(AddCodingReport)}\n" +
                            $"There was an error adding the coding report for coder {report.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(AddCodingReport)}\n" +
                            $"There was an unexpected error adding the coding report for coder {report.CoderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return -1;
        }
    }


    public CodingReport? GetCodingReport(int coderId, int reportId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new { Id = reportId, CoderId = coderId };

                return connection.QuerySingleOrDefault<CodingReport>(CodingReportStatements.GetCodingReport,
                    parameters);
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(GetCodingReport)}\n" +
                            $"There was an error retrieving the coding report for coder {coderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(GetCodingReport)}\n" +
                            $"There was an unexpected error retrieving the coding report for coder {coderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public List<CodingReport> GetCodingReports(int coderId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString.Value))
            {
                connection.Open();

                var parameters = new { CoderId = coderId };

                return connection.Query<CodingReport>(CodingReportStatements.GetCodingReports, parameters)
                    .ToList();
            }
        }
        catch (SqliteException ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(GetCodingReports)}\n" +
                            $"There was an error retrieving the coding reports for coder {coderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CodingReportRepository)}\nMethod: {nameof(GetCodingReports)}\n" +
                            $"There was an unexpected error retrieving the coding reports for coder {coderId}" +
                            $" to the database: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}