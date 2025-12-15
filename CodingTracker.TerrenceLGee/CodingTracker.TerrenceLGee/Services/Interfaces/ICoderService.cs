using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;

namespace CodingTracker.TerrenceLGee.Services.Interfaces;

public interface ICoderService
{
    int AddCoder(CreateCoderDto dto);
    RetrievedCoderDto? GetCoder(string firstName, string lastName);
    bool CoderAlreadyExists(string firstName, string lastName);
    CreateCodingReportDto? GenerateCodingReport(RetrievedCoderDto dto);
}