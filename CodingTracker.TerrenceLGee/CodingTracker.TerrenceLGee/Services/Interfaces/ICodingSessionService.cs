using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;

namespace CodingTracker.TerrenceLGee.Services.Interfaces;

public interface ICodingSessionService
{
    int AddCodingSession(CreateCodingSessionDto dto);
    int UpdateCodingSession(UpdateCodingSessionDto dto);
    int DeleteCodingSession(int goalId, int sessionId);
    RetrievedCodingSessionDto? GetCodingSession(int goalId, int sessionId);
    List<RetrievedCodingSessionDto> GetCodingSessions(int goalId);
}