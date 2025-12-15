using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

namespace CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

public interface ICodingReportUi
{
    void GenerateReport(RetrievedCoderDto dto);
}