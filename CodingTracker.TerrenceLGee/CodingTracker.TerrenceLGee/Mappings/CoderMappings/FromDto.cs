using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Mappings.CoderMappings;

public static class FromDto
{
    extension(CreateCoderDto dto)
    {
        public Coder FromCreateCoderDto()
        {
            return new Coder
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
        }
    }
}