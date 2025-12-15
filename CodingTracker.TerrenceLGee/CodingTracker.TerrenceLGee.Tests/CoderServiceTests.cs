using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Models;
using CodingTracker.TerrenceLGee.Services;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using Moq;

namespace CodingTracker.TerrenceLGee.Tests;

public class CoderServiceTests
{
    private readonly Mock<ICoderRepository> _mockRepo;
    private readonly ICoderService _coderService;
    private const string First = "Bob";
    private const string Last = "Smith";

    public CoderServiceTests()
    {
        _mockRepo = new Mock<ICoderRepository>();
        _coderService = new CoderService(_mockRepo.Object);
    }
    
    [Fact]
    public void AddCoder_ShouldReturn1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.AddCoder(It.IsAny<Coder>()))
            .Returns(expectedResult);

        var result = _coderService.AddCoder(new CreateCoderDto());
        
        Assert.Equal(expectedResult, result);
    }
   

    [Fact]
    public void AddCoder_ShouldReturn0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.AddCoder(It.IsAny<Coder>()))
            .Returns(expectedResult);

        var result = _coderService.AddCoder(new CreateCoderDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetCoder_ShouldReturnCoder_WhenCoderExists()
    {
        var expectedResult = new Coder { FirstName = First, LastName = Last };
        _mockRepo
            .Setup(r => r.GetCoder(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedResult);

        var result = _coderService.GetCoder(First, Last);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.FirstName, result.FirstName);
    }

    [Fact]
    public void GetCoder_ShouldReturnNull_WhenCoderDoesNotExist()
    {
        Coder? expectedResult = null;
        _mockRepo
            .Setup(r => r.GetCoder(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedResult);

        var result = _coderService.GetCoder(First, Last);
        Assert.Null(result);
    }

    
    [Fact]
    public void CoderAlreadyExists_ShouldReturnTrue_WhenCoderExists()
    {
        const bool expectedResult = true;
        _mockRepo
            .Setup(r => r.CoderAlreadyExists(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedResult);

        var result = _coderService.CoderAlreadyExists(First, Last);
        Assert.True(result);
    }

    [Fact]
    public void CoderAlreadyExists_ShouldReturnFalse_WhenCoderDoesNotExist()
    {
        const bool expectedResult = false;
        _mockRepo
            .Setup(r => r.CoderAlreadyExists(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedResult);

        var result = _coderService.CoderAlreadyExists(First, Last);
        Assert.False(result);
    }

    [Fact]
    public void GenerateCodingReport_ShouldReturnReport_WhenCoderHasGoals()
    {
        var expectedResult = new CreateCodingReportDto { TotalGoals = 1 };
        var mockService = new Mock<ICoderService>();

        var coder = new RetrievedCoderDto
        {
            Id = 1,
            FirstName = First,
            LastName = Last,
            Goals =
            [
                new RetrievedCodingGoalDto
                {
                    Id = 1,
                    CoderId = 1,
                    StartDate = new DateTime(2025, 12, 1),
                    EndDate = new DateTime(2025, 12, 7),
                    GoalHours = 56,
                    HoursCodedSoFar = 56,
                    HoursNeededToReachGoal = 8,
                    IsCurrentCodingGoal = true,
                    IsEndDateExpired = false,
                    IsGoalMet = true,
                    IsGoalFinished = true
                }
            ]
        };

        mockService
            .Setup(s => s.GenerateCodingReport(It.IsAny<RetrievedCoderDto>()))
            .Returns(expectedResult);

        var result = mockService.Object.GenerateCodingReport(coder);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.TotalGoals, result.TotalGoals);
    }

    [Fact]
    public void GenerateCodingReport_ShouldReturnNull_WhenCoderHasNoGoals()
    {
        CreateCodingReportDto? expectedResult = null;
        var mockService = new Mock<ICoderService>();

        mockService
            .Setup(s => s.GenerateCodingReport(It.IsAny<RetrievedCoderDto>()))
            .Returns(expectedResult);

        var coder = new RetrievedCoderDto
        {
            Id = 1,
            FirstName = First,
            LastName = Last,
            Goals = [],
        };

        var result = mockService.Object.GenerateCodingReport(coder);
        
        Assert.Null(result);
    }
}