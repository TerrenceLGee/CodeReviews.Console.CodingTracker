using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;

namespace CodingTracker.TerrenceLGee.Services;

public class CodingGoalService : ICodingGoalService
{
    private readonly ICodingGoalRepository _repository;

    public CodingGoalService(ICodingGoalRepository repository) => _repository = repository;


    public int AddCodingGoal(CreateCodingGoalDto dto)
    {
        return _repository.AddCodingGoal(dto.FromCreateCodingGoalDto());
    }

    public int UpdateCodingGoal(UpdateCodingGoalDto dto)
    {
        return _repository.UpdateCodingGoal(dto.FromUpdateCodingGoalDto());
    }

    public bool HasCodingGoals(int coderId)
    {
        return _repository.HasCodingGoals(coderId);
    }

    public RetrievedCodingGoalDto? GetCurrentCodingGoal(int coderId)
    {
        var goal = _repository.GetCurrentCodingGoal(coderId);
        return goal?.ToRetrievedCodingGoalDto();
    }

    public RetrievedCodingGoalDto? GetCodingGoal(int coderId, int goalId)
    {
        var goal = _repository.GetCodingGoal(coderId, goalId);
        return goal?.ToRetrievedCodingGoalDto();
    }

    public List<RetrievedCodingGoalDto> GetCodingGoals(int coderId)
    {
        var goals = _repository.GetCodingGoals(coderId);
        return goals.ToRetrievedCodingGoalDtos();
    }
}