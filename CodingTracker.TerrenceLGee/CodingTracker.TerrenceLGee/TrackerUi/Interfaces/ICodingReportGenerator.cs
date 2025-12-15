using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

namespace CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

public interface ICodingReportGenerator
{
    bool GenerateNewCodingReport(RetrievedCoderDto dto);
}