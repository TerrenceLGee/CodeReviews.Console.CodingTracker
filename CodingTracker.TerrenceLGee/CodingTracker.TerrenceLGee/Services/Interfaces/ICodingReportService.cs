using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;

namespace CodingTracker.TerrenceLGee.Services.Interfaces;

public interface ICodingReportService
{
    int AddCodingReport(CreateCodingReportDto dto);
    RetrievedCodingReportDto? GetCodingReport(int coderId, int reportId);
    List<RetrievedCodingReportDto> GetCodingReports(int coderId);
}