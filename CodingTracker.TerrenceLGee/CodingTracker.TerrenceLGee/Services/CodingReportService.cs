using CodingTracker.TerrenceLGee.Data.Interfaces;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingReportMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;

namespace CodingTracker.TerrenceLGee.Services;

public class CodingReportService : ICodingReportService
{
    private readonly ICodingReportRepository _repository;

    public CodingReportService(ICodingReportRepository repository) => _repository = repository;


    public int AddCodingReport(CreateCodingReportDto dto)
    {
        return _repository.AddCodingReport(dto.FromCreateCodingReportDto());
    }

    public RetrievedCodingReportDto? GetCodingReport(int coderId, int reportId)
    {
        var codingReport = _repository.GetCodingReport(coderId, reportId);
        return codingReport?.ToRetrievedCodingReportDto();
    }

    public List<RetrievedCodingReportDto> GetCodingReports(int coderId)
    {
        var reports = _repository.GetCodingReports(coderId);
        return reports.ToRetrievedCodingReportDtos();
    }
}