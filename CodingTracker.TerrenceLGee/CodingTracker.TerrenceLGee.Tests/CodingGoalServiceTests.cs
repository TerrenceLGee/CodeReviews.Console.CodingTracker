using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.Models;
using CodingTracker.TerrenceLGee.Services;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using Moq;

namespace CodingTracker.TerrenceLGee.Tests;

public class CodingGoalServiceTests
{
    private readonly Mock<ICodingGoalRepository> _mockRepo;
    private readonly ICodingGoalService _goalService;
    private const int CoderId = 1;
    private const int GoalId = 3;

    public CodingGoalServiceTests()
    {
        _mockRepo = new Mock<ICodingGoalRepository>();
        _goalService = new CodingGoalService(_mockRepo.Object);
    }

    [Fact]
    public void AddCodingGoal_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.AddCodingGoal(It.IsAny<CodingGoal>()))
            .Returns(expectedResult);

        var result = _goalService.AddCodingGoal(new CreateCodingGoalDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddCodingGoal_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.AddCodingGoal(It.IsAny<CodingGoal>()))
            .Returns(expectedResult);

        var result = _goalService.AddCodingGoal(new CreateCodingGoalDto());
        
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void UpdateGoal_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.UpdateCodingGoal(It.IsAny<CodingGoal>()))
            .Returns(expectedResult);

        var result = _goalService.UpdateCodingGoal(new UpdateCodingGoalDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void UpdateCodingGoal_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.UpdateCodingGoal(It.IsAny<CodingGoal>()))
            .Returns(expectedResult);

        var result = _goalService.UpdateCodingGoal(new UpdateCodingGoalDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void HasCodingGoals_ReturnsTrue_WhenCoderHasGoals()
    {
        const bool expectedResult = true;
        _mockRepo
            .Setup(r => r.HasCodingGoals(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.HasCodingGoals(CoderId);
        
        Assert.True(result);
    }

    [Fact]
    public void HasCodingGoals_ReturnsFalse_WhenCoderHasNoGoals()
    {
        const bool expectedResult = false;
        _mockRepo
            .Setup(r => r.HasCodingGoals(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.HasCodingGoals(CoderId);
        
        Assert.False(result);
    }

    [Fact]
    public void GetCurrentCodingGoal_ReturnsCurrentGoal_WhenCoderHasCurrentGoal()
    {
        var expectedResult = new CodingGoal { Id = GoalId, CoderId = CoderId, IsCurrentCodingGoal = true };
        _mockRepo
            .Setup(r => r.GetCurrentCodingGoal(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCurrentCodingGoal(CoderId);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.Id, result.Id);
    }

    [Fact]
    public void GetCurrentCodingGoal_ReturnsNull_WhenCoderHasNoCurrentGoal()
    {
        CodingGoal? expectedResult = null;
        _mockRepo
            .Setup(r => r.GetCurrentCodingGoal(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCurrentCodingGoal(CoderId);
        
        Assert.Null(result);
    }
    
    [Fact]
    public void GetCodingGoal_ReturnsGoal_WhenGoalExists()
    {
        var expectedResult = new CodingGoal { Id = GoalId, CoderId = CoderId, IsGoalMet = true };
        _mockRepo
            .Setup(r => r.GetCodingGoal(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCodingGoal(CoderId, GoalId);

        Assert.NotNull(result);
        Assert.Equal(expectedResult.Id, result.Id);
    }

    [Fact]
    public void GetCodingGoal_ReturnsNull_WhenGoalDoesNotExist()
    {
        CodingGoal? expectedResult = null;
        _mockRepo
            .Setup(r => r.GetCodingGoal(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCodingGoal(CoderId, GoalId);
        
        Assert.Null(result);
    }

    [Fact]
    public void GetCodingGoals_ReturnsListOfGoals_WhenCoderHasGoals()
    {
        var expectedResult = new List<CodingGoal>
        {
            new() {Id = GoalId, CoderId = CoderId, IsGoalMet = true},
            new() {Id = GoalId + 1, CoderId = CoderId, IsGoalMet = true},
            new()
            {
                Id = GoalId + 2,
                CoderId = CoderId,
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2026, 1, 1),
                GoalHours = 248,
                HoursCodedSoFar = 112,
                HoursNeededToReachGoal = 8,
                IsCurrentCodingGoal = true,
                IsEndDateExpired = false,
                IsGoalMet = false,
                IsGoalFinished = false,
                ActualEndDate = null
            },
        };
        _mockRepo
            .Setup(r => r.GetCodingGoals(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCodingGoals(CoderId);
        
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(GoalId, result[0].Id);
        Assert.Equal(GoalId + 1, result[1].Id);
        Assert.True(result[1].IsGoalMet);
        Assert.False(result[2].IsGoalMet);
    }

    [Fact]
    public void GetCodingGoals_ReturnsEmptyList_WhenCoderHasNoGoals()
    {
        List<CodingGoal> expectedResult = [];
        _mockRepo
            .Setup(r => r.GetCodingGoals(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _goalService.GetCodingGoals(CoderId);
        
        Assert.Empty(result);
    }
}