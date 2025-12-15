using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Models;
using CodingTracker.TerrenceLGee.Services;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using Moq;

namespace CodingTracker.TerrenceLGee.Tests;

public class CodingSessionServiceTests
{
    private readonly Mock<ICodingSessionRepository> _mockRepo;
    private readonly ICodingSessionService _sessionService;
    private const int GoalId = 1;
    private const int SessionId = 3;

    public CodingSessionServiceTests()
    {
        _mockRepo = new Mock<ICodingSessionRepository>();
        _sessionService = new CodingSessionService(_mockRepo.Object);
    }

    [Fact]
    public void AddCodingSession_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.AddCodingSession(It.IsAny<CodingSession>()))
            .Returns(expectedResult);

        var result = _sessionService.AddCodingSession(new CreateCodingSessionDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void AddCodingSession_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.AddCodingSession(It.IsAny<CodingSession>()))
            .Returns(expectedResult);

        var result = _sessionService.AddCodingSession(new CreateCodingSessionDto());
        
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void UpdateCodingSession_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.UpdateCodingSession(It.IsAny<CodingSession>()))
            .Returns(expectedResult);

        var result = _sessionService.UpdateCodingSession(new UpdateCodingSessionDto());
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void UpdateCodingSession_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.UpdateCodingSession(It.IsAny<CodingSession>()))
            .Returns(expectedResult);

        var result = _sessionService.UpdateCodingSession(new UpdateCodingSessionDto());
        
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void DeleteCodingSession_Returns1_WhenSuccessful()
    {
        const int expectedResult = 1;
        _mockRepo
            .Setup(r => r.DeleteCodingSession(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.DeleteCodingSession(GoalId, SessionId);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void DeleteCodingSession_Returns0_WhenFailed()
    {
        const int expectedResult = 0;
        _mockRepo
            .Setup(r => r.DeleteCodingSession(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.DeleteCodingSession(GoalId, SessionId);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetCodingSession_ReturnsCodingSession_WhenSessionExists()
    {
        var expectedResult = new CodingSession
        {
            Id = SessionId, 
            GoalId = GoalId, 
            StartTime = new DateTime(2025, 12, 1, 13, 12, 0),
            EndTime = new DateTime(2025, 12, 1, 21, 12, 0),
            IsSessionFinished = true
        };
        
        _mockRepo
            .Setup(r => r.GetCodingSession(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.GetCodingSession(GoalId, SessionId);

        Assert.NotNull(result);
        Assert.True(result.IsSessionFinished);
        Assert.Equal(expectedResult.Id, result.Id);
        Assert.NotNull(result.SessionDuration);
        Assert.Equal(8, result.SessionDuration.Value.Hours);
    }

    [Fact]
    public void GetCodingSession_ReturnsNull_WhenSessionDoesNotExist()
    {
        CodingSession? expectedResult = null;
        _mockRepo.Setup(r => r.GetCodingSession(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.GetCodingSession(GoalId, SessionId);
        
        Assert.Null(result);
    }

    [Fact]
    public void GetCodingSessions_ReturnsListOfSessions_WhenGoalHasSessions()
    {
        var expectedResult = new List<CodingSession>
        {
            new()
            {
                Id = SessionId, 
                GoalId = GoalId, 
                StartTime = new DateTime(2025, 12, 1, 13, 12, 0),
                EndTime = new DateTime(2025, 12, 1, 21, 12, 0),
                IsSessionFinished = true
            },
            new()
            {
                Id = SessionId + 1, 
                GoalId = GoalId, 
                StartTime = new DateTime(2025, 12, 2, 13, 12, 0),
                EndTime = new DateTime(2025, 12, 2, 16, 12, 0),
                IsSessionFinished = true
            },
            new()
            {
                Id = SessionId + 2, 
                GoalId = GoalId, 
                StartTime = new DateTime(2025, 12, 1, 13, 12, 0),
                EndTime = null,
                IsSessionFinished = false
            }
        };

        _mockRepo
            .Setup(r => r.GetCodingSessions(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.GetCodingSessions(GoalId);
        
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
        Assert.True(result[0].IsSessionFinished);
        Assert.True(result[1].IsSessionFinished);
        Assert.False(result[2].IsSessionFinished);
        Assert.NotNull(result[1].SessionDuration);
        Assert.Null(result[2].SessionDuration);
    }

    [Fact]
    public void GetCodingSessions_ReturnsEmptyList_WhenGoalHasNoSessions()
    {
        List<CodingSession> expectedResult = [];

        _mockRepo
            .Setup(r => r.GetCodingSessions(It.IsAny<int>()))
            .Returns(expectedResult);

        var result = _sessionService.GetCodingSessions(GoalId);
        
        Assert.Empty(result);
    }
}