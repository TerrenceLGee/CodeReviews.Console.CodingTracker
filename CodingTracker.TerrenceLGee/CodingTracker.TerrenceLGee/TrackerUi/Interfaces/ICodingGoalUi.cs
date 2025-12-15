using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

namespace CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

public interface ICodingGoalUi
{
    void Initialize(RetrievedCoderDto dto);
    bool AddCodingGoal(RetrievedCoderDto dto);
    bool ResetGoal(RetrievedCoderDto dto);
}