using System.Globalization;
using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Helpers;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using Spectre.Console;

using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CodingGoalUi : ICodingGoalUi
{
    private readonly ICodingGoalService _service;
    private readonly ICodingReportGenerator _reportGenerator;

    public CodingGoalUi(
        ICodingGoalService service,
        ICodingReportGenerator reportGenerator)
    {
        _service = service;
        _reportGenerator = reportGenerator;
    }


    public void Initialize(RetrievedCoderDto dto)
    {
        var doesUserHaveAnyCodingGoals = _service.HasCodingGoals(dto.Id);
        var doesUserHaveCurrentCodingGoal = false;
        var hasEndDateExpired = false;
        var hasGoalBeenMet = false;

        if (!doesUserHaveAnyCodingGoals)
        {
            InputHelpers
                .PressAnyKeyToContinue($"{dto.FirstName}, " +
                                       $"you currently have no coding goals, " +
                                       $"so we need to add one first");
            AddCodingGoal(dto);
            return;
        }

        var currentCodingGoal = _service.GetCurrentCodingGoal(dto.Id);
        doesUserHaveCurrentCodingGoal = currentCodingGoal is not null;

        if (doesUserHaveCurrentCodingGoal)
        {
            hasEndDateExpired = currentCodingGoal!.IsEndDateExpired;
            hasGoalBeenMet = currentCodingGoal!.IsGoalMet;
            if (hasGoalBeenMet || hasEndDateExpired)
            {
                currentCodingGoal.IsCurrentCodingGoal = false;
            }
        }

        if (!doesUserHaveCurrentCodingGoal || hasGoalBeenMet || hasEndDateExpired)
        {
            InputHelpers
                .PressAnyKeyToContinue($"{dto.FirstName}, " +
                                       $"you do not have a current coding goal, " +
                                       $"so let's add a new goal");
            AddCodingGoal(dto);
        }
    }

    public bool AddCodingGoal(RetrievedCoderDto dto)
    {
        if (dto.CurrentCodingGoal is not null)
        {
            if (!ResetGoal(dto))
            {
                InputHelpers
                    .PressAnyKeyToContinueError($"{dto.FirstName}, there was an unexpected problem resetting your current coding goal status");
                return false;
            }
        }
        
        InputHelpers.PressAnyKeyToContinue($"\n{dto.FirstName}, let's begin adding your coding goal\n");
        var (start, end) = GetValidDates();

        var correctChoice = false;
        var goalCodingHours = 0;

        while (!correctChoice || goalCodingHours <= 0)
        {
            goalCodingHours =
                AnsiConsole
                    .Ask<int>($"[{GetRandomColor()}]" +
                              $"Enter the total amount of hours you want to code during this time period " +
                              $"(greater than 0): [/]");
            correctChoice = InputHelpers
                .GetOptionalInput(
                    $"You entered {goalCodingHours} for the number of hours to code for this goal, is this correct? ");
        }

        var goal = new CreateCodingGoalDto
        {
            CoderId = dto.Id,
            StartDate = start,
            EndDate = end,
            GoalHours = goalCodingHours,
            IsCurrentCodingGoal = true
        };

        if (_service.AddCodingGoal(goal) != 1)
        {
            return false;
        }

        dto.Goals = _service.GetCodingGoals(dto.Id);
        dto.CurrentCodingGoal = _service.GetCurrentCodingGoal(dto.Id);

        if (dto.CurrentCodingGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinue($"{dto.FirstName}, " +
                                       $"there was an error adding your coding goal" +
                                       $"\nReturning to previous menu");
            return false;
        }

        if (!dto.CurrentCodingGoal.IsCurrentCodingGoal)
        {
            dto.CurrentCodingGoal.IsCurrentCodingGoal = true;
        }

        var generateReport = InputHelpers.GetOptionalInput(
            $"{dto.FirstName}, you can also generate a coding report to be " +
            $"viewed from the main menu, would you like to do so? ");

        if (generateReport)
        {
            if (!_reportGenerator.GenerateNewCodingReport(dto))
            {
                InputHelpers
                    .PressAnyKeyToContinueError("Unable to generate a coding report at this time, but your goal was added");
                return true;
            }
            InputHelpers.PressAnyKeyToContinue($"{dto.FirstName}, you will be able to view your report from the main menu");
        }

        return true;
    }

    public bool ResetGoal(RetrievedCoderDto dto)
    {
        if (dto.CurrentCodingGoal is null) return false;
        dto.CurrentCodingGoal.IsCurrentCodingGoal = false;
        return _service
            .UpdateCodingGoal(dto.CurrentCodingGoal.
                FromRetrievedCodingGoalDtoToUpdateCodingGoalDto()) == 1;
    }

    private static DateTime GetDateForGoal(string range, string dateFormat)
    {
        DateTime date;
        var correctChoice = false;

        do
        {
            var dateString = AnsiConsole
                .Ask<string>($"[{GetRandomColor()}]" +
                             $"Please enter {range} in format: {dateFormat} " +
                             $"(Date cannot be in the past): [/]")
                .Trim();

            while (!DateTime.TryParseExact(
                       dateString,
                       dateFormat,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out date))
            {
                dateString = AnsiConsole
                    .Ask<string>($"[{GetRandomColor()}]" +
                                 $"Please enter {range} in format: {dateFormat} " +
                                 $"(Date cannot be in the past): [/]")
                    .Trim();
            }

            correctChoice =
                InputHelpers.GetOptionalInput(
                    $"The {range} date you entered is {date.ToString(dateFormat)}, is this correct? ");
        } while (!DateTimeHelpers.IsValidDate(date) || !correctChoice);

        return date;
    }

    private static (DateTime start, DateTime end) GetValidDates()
    {
        var start = DateTime.MaxValue;
        var end = DateTime.MinValue;

        while (!DateTimeHelpers.IsValidEndDate(start, end))
        {
            start = GetDateForGoal("beginning date", InputHelpers.DateFormat);
            end = GetDateForGoal("ending date", InputHelpers.DateFormat);

            if (!DateTimeHelpers.IsValidEndDate(start, end))
            {
                InputHelpers.PressAnyKeyToContinueError("Beginning date must come before ending date\n");
            }
        }

        return (start, end);
    }
}