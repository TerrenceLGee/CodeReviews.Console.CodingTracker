using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Data.Interfaces;

public interface ICodingReportRepository
{
    int AddCodingReport(CodingReport report);
    CodingReport? GetCodingReport(int coderId, int reportId);
    List<CodingReport> GetCodingReports(int coderId);
}