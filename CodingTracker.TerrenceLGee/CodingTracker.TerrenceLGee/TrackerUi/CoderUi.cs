using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using Spectre.Console;

using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CoderUi : ICoderUi
{
    private readonly ICoderService _service;
    
    public CoderUi(ICoderService service) => _service = service;
    
    public RetrievedCoderDto? GetCoder()
    {
        var firstName = AnsiConsole.Ask<string>($"[{GetRandomColor()}]Please enter your first name: [/]");
        var lastName = AnsiConsole.Ask<string>($"[{GetRandomColor()}]Please enter your last name: [/]");

        if (_service.CoderAlreadyExists(firstName, lastName))
        {
            return _service.GetCoder(firstName, lastName);
        }

        var coder = new CreateCoderDto
        {
            FirstName = firstName,
            LastName = lastName
        };

        return _service.AddCoder(coder) == 1
            ? _service.GetCoder(firstName, lastName)
            : null;
    }
}