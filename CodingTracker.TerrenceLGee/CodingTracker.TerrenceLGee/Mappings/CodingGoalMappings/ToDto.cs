using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingSessionMappings;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;

public static class ToDto
{
    extension(CodingGoal goal)
    {
        public RetrievedCodingGoalDto ToRetrievedCodingGoalDto()
        {
            return new RetrievedCodingGoalDto
            {
                Id = goal.Id,
                CoderId = goal.CoderId,
                StartDate = goal.StartDate,
                EndDate = goal.EndDate,
                GoalHours = goal.GoalHours,
                HoursNeededToReachGoal = goal.HoursNeededToReachGoal,
                HoursCodedSoFar = goal.GetHoursCodedSoFar(),
                IsCurrentCodingGoal = goal.IsCurrentCodingGoal,
                IsGoalMet = goal.GetIsGoalMet(),
                IsEndDateExpired = goal.GetIsEndDateExpired(),
                IsGoalFinished = goal.GetIsGoalFinished(),
                ActualEndDate = goal.IsGoalFinished ? DateTime.Now : null,
                Sessions = goal.GetSessions()
            };
        }
        
        private List<RetrievedCodingSessionDto> GetSessions()
        {
            return goal.Sessions
                .Select(s => s.ToRetrievedCodingSessionDto())
                .ToList();
        }

        private int GetHoursCodedSoFar()
        {
            var hours = goal.Sessions
                .Sum(s => s.SessionDuration?.Hours ?? 0);
            var minutes = goal.Sessions
                .Sum(s => s.SessionDuration?.Minutes ?? 0);
            
            var minutesToHours = minutes / 60;
            return hours + minutesToHours;
        }

        private bool GetIsGoalMet()
        {
            return goal.GetHoursCodedSoFar() >= goal.GoalHours;
        }
        
        private bool GetIsEndDateExpired()
        {
            return DateTime.Now.Day > goal.EndDate.Day;
        }
        
        private bool GetIsGoalFinished() 
        {
            return goal.GetIsGoalMet() || goal.GetIsEndDateExpired();
        }
    }

    extension(List<CodingGoal> goals)
    {
        public List<RetrievedCodingGoalDto> ToRetrievedCodingGoalDtos()
        {
            return goals
                .Select(s => s.ToRetrievedCodingGoalDto())
                .ToList();
        }
    }
}