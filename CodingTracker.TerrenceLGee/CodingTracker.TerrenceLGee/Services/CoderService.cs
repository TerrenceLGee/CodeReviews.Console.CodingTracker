using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Mappings.CoderMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;

namespace CodingTracker.TerrenceLGee.Services;

public class CoderService : ICoderService
{
    private readonly ICoderRepository _repository;

    public CoderService(ICoderRepository repository) => _repository = repository;


    public int AddCoder(CreateCoderDto dto)
    {
        return _repository.AddCoder(dto.FromCreateCoderDto());
    }

    public RetrievedCoderDto? GetCoder(string firstName, string lastName)
    {
        var coder = _repository.GetCoder(firstName, lastName);
        return coder?.ToRetrievedCoderDto();
    }

    public bool CoderAlreadyExists(string firstName, string lastName)
    {
        return _repository.CoderAlreadyExists(firstName, lastName);
    }

    public CreateCodingReportDto? GenerateCodingReport(RetrievedCoderDto dto)
    {
        if (dto.Goals.Count == 0) return null;

        var targetDaysSetForGoal = dto.Goals
            .Sum(g => (g.EndDate.Day - g.StartDate.Day) + 1);
        var totalDaysSetForGoal = dto.Goals
            .Sum(g => g.StartDate.Day - (g.ActualEndDate?.Day ?? 0) + 1);
        
        var totalHoursWantingToCode = dto.Goals
            .Sum(g => g.GoalHours);
        var totalHoursNeededToCodeToReachGoal = dto.Goals
            .Sum(g => g.HoursNeededToReachGoal);
        var totalHoursActuallyCoded = dto.Goals.Sum(g => g.HoursCodedSoFar);
        
        var howManyEndDateExpired = dto.Goals
            .Where(g => g is { IsGoalFinished: true, IsGoalMet: false })
            .Count(g => g.IsEndDateExpired);
        var howManyGoalMet = dto.Goals.Count(g => g.IsGoalMet);
        
        var sessions = dto.Goals.SelectMany(g => g.Sessions)
            .Select(s => s).ToList();
        var totalSessionsDuration = new TimeSpan(sessions.Sum(s => s.SessionDuration?.Ticks ?? 0));
        var numberOfFinishedSessions = sessions.Count(s => s.IsSessionFinished);
        var totalGoals = dto.Goals.Count;
        var totalSessions = dto.Goals.Sum(g => g.Sessions.Count);

        return new CreateCodingReportDto
        {
            CoderId = dto.Id,
            TargetDaysSetForGoal = targetDaysSetForGoal,
            TotalDaysSetForGoal = totalDaysSetForGoal,
            TotalHoursWantingToCode = totalHoursWantingToCode,
            TotalHoursNeededToCodeToReachGoal = totalHoursNeededToCodeToReachGoal,
            TotalHoursActuallyCoded = totalHoursActuallyCoded,
            HowManyEndDateExpired = howManyEndDateExpired,
            HowManyGoalMet = howManyGoalMet,
            TotalSessionsDuration = totalSessionsDuration,
            NumberOfFinishedSessions = numberOfFinishedSessions,
            ReportGenerated = DateTime.Now,
            TotalGoals = totalGoals,
            TotalSessions = totalSessions
        };
    }
}