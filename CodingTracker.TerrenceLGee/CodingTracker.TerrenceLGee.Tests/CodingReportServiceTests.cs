using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Models;
using CodingTracker.TerrenceLGee.Services;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using Moq;

namespace CodingTracker.TerrenceLGee.Tests;

public class CodingReportServiceTests
{
    private readonly Mock<ICodingReportRepository> _mockRepo;
    private readonly ICodingReportService _reportService;
    private const int CoderId = 1;
    private const int ReportId = 3;

    public CodingReportServiceTests()
    {
        _mockRepo = new Mock<ICodingReportRepository>();
        _reportService = new CodingReportService(_mockRepo.Object);
    }

    [Fact]
    public void AddCodingReport_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo.Setup(r => r.AddCodingReport(It.IsAny<CodingReport>()))
            .Returns(expectedResult);

        var result = _reportService.AddCodingReport(new CreateCodingReportDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddCodingReport_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo.Setup(r => r.AddCodingReport(It.IsAny<CodingReport>()))
            .Returns(expectedResult);

        var result = _reportService.AddCodingReport(new CreateCodingReportDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetCodingReport_ReturnsReport_WhenReportExists()
    {
        var expectedResult = new CodingReport
        {
            Id = ReportId,
            CoderId = CoderId,
            TargetDaysSetForGoal = 30,
            TotalDaysSetForGoal = 30,
            TotalHoursWantingToCode = 248,
            TotalHoursNeededToCodeToReachGoal = 248,
            TotalHoursActuallyCoded = 200,
            HowManyEndDateExpired = 0,
            HowManyGoalMet = 3,
            TotalSessionsDuration = new TimeSpan(200, 12, 0),
            NumberOfFinishedSessions = 3,
            ReportGenerated = DateTime.Now.Date,
            TotalGoals = 3,
            TotalSessions = 24
        };

        _mockRepo.Setup(r => r.GetCodingReport(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _reportService.GetCodingReport(CoderId, ReportId);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.Id, result.Id);
        Assert.Equal(expectedResult.TotalGoals, result.TotalGoals);
        Assert.Equal(expectedResult.TotalSessionsDuration.Hours, result.TotalSessionsDuration.Hours);
    }

    [Fact]
    public void GetCodingReport_ReturnsNull_WhenReportDoesNotExist()
    {
        CodingReport? expectedResult = null;
        _mockRepo.Setup(r => r.GetCodingReport(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _reportService.GetCodingReport(CoderId, ReportId);
        
        Assert.Null(result);
    }

    [Fact]
    public void GetCodingReports_ReturnsListOfReports_WhenCoderHasReports()
    {
        var expectedResult = new List<CodingReport>
        {
            new()
            {
                Id = ReportId,
                CoderId = CoderId,
                TargetDaysSetForGoal = 30,
                TotalDaysSetForGoal = 30,
                TotalHoursWantingToCode = 248,
                TotalHoursNeededToCodeToReachGoal = 248,
                TotalHoursActuallyCoded = 200,
                HowManyEndDateExpired = 0,
                HowManyGoalMet = 3,
                TotalSessionsDuration = new TimeSpan(200, 12, 0),
                NumberOfFinishedSessions = 3,
                ReportGenerated = DateTime.Now.Date,
                TotalGoals = 3,
                TotalSessions = 24
            },
            new()
            {
                Id = ReportId + 1,
                CoderId = CoderId,
                TargetDaysSetForGoal = 30,
                TotalDaysSetForGoal = 30,
                TotalHoursWantingToCode = 248,
                TotalHoursNeededToCodeToReachGoal = 248,
                TotalHoursActuallyCoded = 200,
                HowManyEndDateExpired = 0,
                HowManyGoalMet = 3,
                TotalSessionsDuration = new TimeSpan(200, 12, 0),
                NumberOfFinishedSessions = 3,
                ReportGenerated = DateTime.Now.Date,
                TotalGoals = 3,
                TotalSessions = 24
            },
            new()
            {
                Id = ReportId + 1,
                CoderId = CoderId,
                TargetDaysSetForGoal = 30,
                TotalDaysSetForGoal = 30,
                TotalHoursWantingToCode = 248,
                TotalHoursNeededToCodeToReachGoal = 248,
                TotalHoursActuallyCoded = 200,
                HowManyEndDateExpired = 0,
                HowManyGoalMet = 3,
                TotalSessionsDuration = new TimeSpan(200, 12, 0),
                NumberOfFinishedSessions = 3,
                ReportGenerated = DateTime.Now.Date,
                TotalGoals = 3,
                TotalSessions = 24
            }
        };

        _mockRepo.Setup(r => r.GetCodingReports(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _reportService.GetCodingReports(CoderId);
        
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedResult[1].NumberOfFinishedSessions, result[1].NumberOfFinishedSessions);
    }

    [Fact]
    public void GetCodingReports_ReturnsEmptyList_WhenCoderHasNoReports()
    {
        List<CodingReport> expectedResult = [];
        
        _mockRepo.Setup(r => r.GetCodingReports(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _reportService.GetCodingReports(CoderId);
        
        Assert.Empty(result);
    }
}