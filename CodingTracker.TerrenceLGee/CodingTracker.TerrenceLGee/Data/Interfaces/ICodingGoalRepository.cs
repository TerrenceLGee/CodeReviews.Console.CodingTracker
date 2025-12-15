using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Data.Interfaces;

public interface ICodingGoalRepository
{
    int AddCodingGoal(CodingGoal goal);
    int UpdateCodingGoal(CodingGoal goal);
    bool HasCodingGoals(int coderId);
    CodingGoal? GetCurrentCodingGoal(int coderId);
    CodingGoal? GetCodingGoal(int coderId, int goalId);
    List<CodingGoal> GetCodingGoals(int coderId);
}