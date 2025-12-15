using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

namespace CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

public interface ICodingSessionUi
{
    void AddCodingSession(RetrievedCoderDto dto);
    void UpdateCodingSession(RetrievedCoderDto dto);
    void DeleteCodingSession(RetrievedCoderDto dto);
}