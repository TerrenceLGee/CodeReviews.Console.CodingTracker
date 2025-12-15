using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingSessionMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;

namespace CodingTracker.TerrenceLGee.Services;

public class CodingSessionService : ICodingSessionService
{
    private readonly ICodingSessionRepository _repository;

    public CodingSessionService(ICodingSessionRepository repository) => _repository = repository;


    public int AddCodingSession(CreateCodingSessionDto dto)
    {
        return _repository.AddCodingSession(dto.FromCreateCodingSession());
    }

    public int UpdateCodingSession(UpdateCodingSessionDto dto)
    {
        return _repository.UpdateCodingSession(dto.FromUpdateCodingSessionDto());
    }

    public int DeleteCodingSession(int goalId, int sessionId)
    {
        return _repository.DeleteCodingSession(goalId, sessionId);
    }

    public RetrievedCodingSessionDto? GetCodingSession(int goalId, int sessionId)
    {
        var session = _repository.GetCodingSession(goalId, sessionId);
        return session?.ToRetrievedCodingSessionDto();
    }

    public List<RetrievedCodingSessionDto> GetCodingSessions(int goalId)
    {
        var sessions = _repository.GetCodingSessions(goalId);
        return sessions.ToRetrievedCodingSessionDtos();
    }
}