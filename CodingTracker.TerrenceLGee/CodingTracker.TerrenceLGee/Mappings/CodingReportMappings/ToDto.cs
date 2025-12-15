using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CodingReportMappings;

public static class ToDto
{
    extension(CodingReport report)
    {
        public RetrievedCodingReportDto ToRetrievedCodingReportDto()
        {
            return new RetrievedCodingReportDto
            {
                Id = report.Id,
                CoderId = report.CoderId,
                TargetDaysSetForGoal = report.TargetDaysSetForGoal,
                TotalDaysSetForGoal = report.TotalDaysSetForGoal,
                TotalHoursWantingToCode = report.TotalHoursWantingToCode,
                TotalHoursNeededToCodeToReachGoal = report.TotalHoursNeededToCodeToReachGoal,
                TotalHoursActuallyCoded = report.TotalHoursActuallyCoded,
                HowManyEndDateExpired = report.HowManyEndDateExpired,
                HowManyGoalMet = report.HowManyGoalMet,
                TotalSessionsDuration = report.TotalSessionsDuration,
                NumberOfFinishedSessions = report.NumberOfFinishedSessions,
                ReportGenerated = report.ReportGenerated,
                TotalGoals = report.TotalGoals,
                TotalSessions = report.TotalSessions
            };
        }
    }

    extension(List<CodingReport> reports)
    {
        public List<RetrievedCodingReportDto> ToRetrievedCodingReportDtos()
        {
            return reports
                .Select(r => r.ToRetrievedCodingReportDto())
                .ToList();
        }
    }
}