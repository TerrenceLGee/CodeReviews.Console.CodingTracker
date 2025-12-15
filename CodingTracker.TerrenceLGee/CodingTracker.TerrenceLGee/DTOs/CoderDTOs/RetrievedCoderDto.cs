using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;

namespace CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

public class RetrievedCoderDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public RetrievedCodingGoalDto? CurrentCodingGoal { get; set; }
    public List<RetrievedCodingGoalDto> Goals { get; set; } = [];
    public List<RetrievedCodingReportDto> Reports { get; set; } = [];
}