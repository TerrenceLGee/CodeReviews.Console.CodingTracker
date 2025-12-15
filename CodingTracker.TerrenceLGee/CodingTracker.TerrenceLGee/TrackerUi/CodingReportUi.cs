using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.TrackerUi.Helpers;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CodingReportUi : ICodingReportUi
{
    private readonly ICodingReportGenerator _reportGenerator;
    private readonly IViewInfoUi _viewInfoUi;

    public CodingReportUi(
        ICodingReportGenerator reportGenerator,
        IViewInfoUi viewInfoUi)
    {
        _reportGenerator = reportGenerator;
        _viewInfoUi = viewInfoUi;
    }


    public void GenerateReport(RetrievedCoderDto dto)
    {
        InputHelpers
            .PressAnyKeyToContinue($"{dto.FirstName}, your coding report is being generated");

        if (!_reportGenerator.GenerateNewCodingReport(dto))
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error generating your coding report");
            return;
        }

        var viewReport = InputHelpers
            .GetOptionalInput("Would you like to view your coding report? ");

        if (!viewReport)
        {
            InputHelpers.PressAnyKeyToContinue($"Ok {dto.FirstName}, " +
                                               $"you can always view your latest report from the main " +
                                               $"menu as well as any previous report");
            return;
        }
        
        _viewInfoUi.ViewLatestCodingReport(dto);
    }
}