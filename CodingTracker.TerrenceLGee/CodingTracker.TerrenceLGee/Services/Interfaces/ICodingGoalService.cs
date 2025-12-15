using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;

namespace CodingTracker.TerrenceLGee.Services.Interfaces;

public interface ICodingGoalService
{
    int AddCodingGoal(CreateCodingGoalDto dto);
    int UpdateCodingGoal(UpdateCodingGoalDto dto);
    bool HasCodingGoals(int coderId);
    RetrievedCodingGoalDto? GetCurrentCodingGoal(int coderId);
    RetrievedCodingGoalDto? GetCodingGoal(int coderId, int goalId);
    List<RetrievedCodingGoalDto> GetCodingGoals(int coderId);
}