using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;

namespace CodingTracker.TerrenceLGee.TrackerUi.Interfaces;

public interface IViewInfoUi
{
    void ViewCodingSession(RetrievedCoderDto dto);
    bool ViewCodingSessions(RetrievedCoderDto dto);
    void ViewCurrentCodingGoal(RetrievedCoderDto dto);
    void ViewPreviousCodingGoal(RetrievedCoderDto dto);
    bool ViewPreviousCodingGoals(RetrievedCoderDto dto);
    void ViewLatestCodingReport(RetrievedCoderDto dto);
    bool ViewCodingReports(RetrievedCoderDto dto);
    void ViewAnyCodingReport(RetrievedCoderDto dto);
}