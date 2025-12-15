using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingSessionMappings;

public static class FromDto
{
    extension(CreateCodingSessionDto dto)
    {
        public CodingSession FromCreateCodingSession()
        {
            return new CodingSession
            {
                GoalId = dto.GoalId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SessionDuration = dto.GetSessionDuration(),
                Comments = dto.Comments,
                IsSessionFinished = dto.GetIsSessionFinished()
            };
        }

        private TimeSpan GetSessionDuration()
        {
            return dto.EndTime.HasValue
                ? dto.EndTime.Value - dto.StartTime
                : TimeSpan.Zero;
        }

        private bool GetIsSessionFinished()
        {
            return dto.EndTime.HasValue;
        }
    }

    extension(UpdateCodingSessionDto dto)
    {
        public CodingSession FromUpdateCodingSessionDto()
        {
            return new CodingSession
            {
                Id = dto.Id,
                GoalId = dto.GoalId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SessionDuration = dto.GetSessionDuration(),
                Comments = dto.Comments,
                IsSessionFinished = dto.GetIsSessionFinished()
            };
        }

        private TimeSpan? GetSessionDuration()
        {
            return dto.EndTime.HasValue
                ? dto.EndTime.Value - dto.StartTime
                : null;
        }

        private bool GetIsSessionFinished()
        {
            return dto.EndTime.HasValue;
        }
    }

    extension(RetrievedCodingSessionDto dto)
    {
        public CodingSession FromRetrievedCodingSessionDto()
        {
            return new CodingSession
            {
                Id = dto.Id,
                GoalId = dto.GoalId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                SessionDuration = dto.GetSessionDuration(),
                Comments = dto.Comments,
                IsSessionFinished = dto.GetIsSessionFinished()
            };
        }

        private TimeSpan? GetSessionDuration()
        {
            return dto.EndTime.HasValue
                ? dto.EndTime.Value - dto.StartTime
                : null;
        }

        private bool GetIsSessionFinished()
        {
            return dto.EndTime.HasValue;
        }
    }
}