using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingSessionMappings;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;

public static class FromDto
{
    extension(CreateCodingGoalDto dto)
    {
        public CodingGoal FromCreateCodingGoalDto()
        {
            return new CodingGoal
            {
                CoderId = dto.CoderId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                GoalHours = dto.GoalHours,
                HoursNeededToReachGoal = dto.GetHoursNeededToReachGoal(),
                HoursCodedSoFar = dto.HoursCodedSoFar,
                IsCurrentCodingGoal = dto.IsCurrentCodingGoal,
                IsEndDateExpired = dto.IsEndDateExpired,
                IsGoalMet = dto.IsGoalMet,
                IsGoalFinished = dto.IsGoalFinished,
                ActualEndDate = dto.ActualEndDate,
            };
        }

        private int GetHoursNeededToReachGoal()
        {
            var dividend = 1;
            
            if (dto.EndDate.Day != dto.StartDate.Day)
            {
                dividend = (dto.EndDate.Day - dto.StartDate.Day) + 1;
            }

            return dto.GoalHours / dividend;
        }
    }

    extension(UpdateCodingGoalDto dto)
    {
        public CodingGoal FromUpdateCodingGoalDto()
        {
            return new CodingGoal
            {
                Id = dto.Id,
                CoderId = dto.CoderId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                GoalHours = dto.GoalHours,
                HoursNeededToReachGoal = dto.GetHoursNeededToReachGoal(),
                HoursCodedSoFar = dto.GetHoursCodedSoFar(),
                IsCurrentCodingGoal = dto.IsCurrentCodingGoal,
                IsEndDateExpired = dto.GetIsEndDateExpired(),
                IsGoalMet = dto.GetIsGoalMet(),
                IsGoalFinished = dto.GetIsGoalFinished(),
                ActualEndDate = dto.IsGoalFinished ? DateTime.Now : null,
                Sessions = dto.GetSessions()
            };
        }

        private int GetHoursNeededToReachGoal()
        {
            var dividend = 1;

            if (dto.EndDate.Day != dto.StartDate.Day)
            {
                dividend = (dto.EndDate.Day - dto.StartDate.Day);
            }

            return dto.GoalHours / dividend;
        }

        private List<CodingSession> GetSessions()
        {
            return dto.Sessions
                .Select(s => s.FromRetrievedCodingSessionDto())
                .ToList();
        }

        private int GetHoursCodedSoFar()
        {
            var hours = dto.Sessions
                .Sum(s => s.SessionDuration?.Hours ?? 0);
            var minutes = dto.Sessions
                .Sum(s => s.SessionDuration?.Minutes ?? 0);

            var minutesToHours = minutes / 60;
            return hours + minutesToHours;
        }

        private bool GetIsGoalMet()
        {
            return dto.GetHoursCodedSoFar() >= dto.GoalHours;
        }

        private bool GetIsEndDateExpired()
        {
            return DateTime.Now.Day > dto.EndDate.Day;
        }

        private bool GetIsGoalFinished()
        {
            return dto.GetIsGoalMet() || dto.GetIsEndDateExpired();
        }
    }

    extension(RetrievedCodingGoalDto dto)
    {
        public UpdateCodingGoalDto FromRetrievedCodingGoalDtoToUpdateCodingGoalDto()
        {
            return new UpdateCodingGoalDto
            {
                Id = dto.Id,
                CoderId = dto.CoderId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                GoalHours = dto.GoalHours,
                HoursNeededToReachGoal = dto.GetHoursNeededToReachGoal(),
                HoursCodedSoFar = dto.GetHoursCodedSoFar(),
                IsCurrentCodingGoal = dto.IsCurrentCodingGoal,
                IsEndDateExpired = dto.GetIsEndDateExpired(),
                IsGoalMet = dto.GetIsGoalMet(),
                IsGoalFinished = dto.GetIsGoalFinished(),
                ActualEndDate = dto.IsGoalFinished ? DateTime.Now : null,
                Sessions = dto.Sessions
            };
        }
        
        private int GetHoursNeededToReachGoal()
        {
            var dividend = 1;

            if (dto.EndDate.Day != dto.StartDate.Day)
            {
                dividend = (dto.EndDate.Day - dto.StartDate.Day);
            }

            return dto.GoalHours / dividend;
        }

        private List<CodingSession> GetSessions()
        {
            return dto.Sessions
                .Select(s => s.FromRetrievedCodingSessionDto())
                .ToList();
        }

        public int GetHoursCodedSoFar()
        {
            var hours = dto.Sessions
                .Sum(s => s.SessionDuration?.Hours ?? 0);
            var minutes = dto.Sessions
                .Sum(s => s.SessionDuration?.Minutes ?? 0);

            var minutesToHours = minutes / 60;
            return hours + minutesToHours;
        }

        public bool GetIsGoalMet()
        {
            return dto.GetHoursCodedSoFar() >= dto.GoalHours;
        }

        public bool GetIsEndDateExpired()
        {
            return DateTime.Now.Day > dto.EndDate.Day;
        }

        public bool GetIsGoalFinished()
        {
            return dto.GetIsGoalMet() || dto.GetIsEndDateExpired();
        }
    }

}