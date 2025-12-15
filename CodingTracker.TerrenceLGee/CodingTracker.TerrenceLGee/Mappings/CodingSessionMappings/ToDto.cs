using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingSessionMappings;

public static class ToDto
{
    extension(CodingSession session)
    {
        public RetrievedCodingSessionDto ToRetrievedCodingSessionDto()
        {
            return new RetrievedCodingSessionDto
            {
                Id = session.Id,
                GoalId = session.GoalId,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                SessionDuration = session.GetSessionDuration(),
                Comments = session.Comments,
                IsSessionFinished = session.GetIsSessionFinished()
            };
        }
        
        private TimeSpan? GetSessionDuration()
        {
            return session.EndTime.HasValue
                ? session.EndTime.Value - session.StartTime
                : null;
        }

        private bool GetIsSessionFinished()
        {
            return session.EndTime.HasValue;
        }
    }

    extension(List<CodingSession> sessions)
    {
        public List<RetrievedCodingSessionDto> ToRetrievedCodingSessionDtos()
        {
            return sessions.Select(s => s.ToRetrievedCodingSessionDto())
                .ToList();
        }
    }
}