using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingReportMappings;

public static class FromDto
{
    extension(CreateCodingReportDto dto)
    {
        public CodingReport FromCreateCodingReportDto()
        {
            return new CodingReport
            {
                CoderId = dto.CoderId,
                TargetDaysSetForGoal = dto.TargetDaysSetForGoal,
                TotalDaysSetForGoal = dto.TotalDaysSetForGoal,
                TotalHoursWantingToCode = dto.TotalHoursWantingToCode,
                TotalHoursNeededToCodeToReachGoal = dto.TotalHoursNeededToCodeToReachGoal,
                TotalHoursActuallyCoded = dto.TotalHoursActuallyCoded,
                HowManyEndDateExpired = dto.HowManyEndDateExpired,
                HowManyGoalMet = dto.HowManyGoalMet,
                TotalSessionsDuration = dto.TotalSessionsDuration,
                NumberOfFinishedSessions = dto.NumberOfFinishedSessions,
                ReportGenerated = dto.ReportGenerated,
                TotalGoals = dto.TotalGoals,
                TotalSessions = dto.TotalSessions
            };
        }
    }
}