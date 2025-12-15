using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CodingReportGenerator : ICodingReportGenerator
{
    private readonly ICoderService _coderService;
    private readonly ICodingReportService _reportService;

    public CodingReportGenerator(
        ICoderService coderService, 
        ICodingReportService reportService)
    {
        _coderService = coderService;
        _reportService = reportService;
    }


    public bool GenerateNewCodingReport(RetrievedCoderDto dto)
    {
        var coder = _coderService.GetCoder(dto.FirstName, dto.LastName);
        if (coder is null) return false;
        dto = coder;
        if (dto.Goals.Count == 0) return false;
        var report = _coderService.GenerateCodingReport(dto);
        if (report is null) return false;
        return _reportService
            .AddCodingReport(report) == 1;
    }
}