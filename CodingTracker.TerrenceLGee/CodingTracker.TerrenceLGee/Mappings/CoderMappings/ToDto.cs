using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;
using CodingTracker.TerrenceLGee.Mappings.CodingReportMappings;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CoderMappings;

public static class ToDto
{
    extension(Coder coder)
    {
        public RetrievedCoderDto ToRetrievedCoderDto()
        {
            return new RetrievedCoderDto
            {
                Id = coder.Id,
                FirstName = coder.FirstName,
                LastName = coder.LastName,
                CurrentCodingGoal = coder.CurrentGoal?.ToRetrievedCodingGoalDto(),
                Goals = coder.GetGoals(),
                Reports = coder.GetReports()
            };
        }

        private List<RetrievedCodingGoalDto> GetGoals()
        {
            return coder.Goals
                .Select(g => g.ToRetrievedCodingGoalDto())
                .ToList();
        }

        private List<RetrievedCodingReportDto> GetReports()
        {
            return coder.Reports
                .Select(r => r.ToRetrievedCodingReportDto())
                .ToList();
        }
    }
}