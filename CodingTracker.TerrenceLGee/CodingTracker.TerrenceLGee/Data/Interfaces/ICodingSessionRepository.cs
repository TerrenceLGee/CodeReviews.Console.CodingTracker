using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Data.Interfaces;

public interface ICodingSessionRepository
{
    int AddCodingSession(CodingSession session);
    int UpdateCodingSession(CodingSession session);
    int DeleteCodingSession(int goalId, int sessionId);
    CodingSession? GetCodingSession(int goalId, int sessionId);
    List<CodingSession> GetCodingSessions(int goalId);
}