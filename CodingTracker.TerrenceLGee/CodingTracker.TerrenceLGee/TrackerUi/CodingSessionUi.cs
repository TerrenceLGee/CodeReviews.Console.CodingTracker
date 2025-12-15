using System.Globalization;
using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Mappings.CodingGoalMappings;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Helpers;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using Spectre.Console;

using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CodingSessionUi : ICodingSessionUi
{
    private readonly ICodingSessionService _service;
    private readonly ICodingGoalService _goalService;
    private readonly ICodingGoalUi _goalUi;
    private readonly IViewInfoUi _viewInfoUi;
    private readonly ICodingReportGenerator _reportGenerator;

    public CodingSessionUi(
        ICodingSessionService service,
        ICodingGoalService goalService,
        ICodingGoalUi goalUi,
        IViewInfoUi viewInfoUi,
        ICodingReportGenerator reportGenerator)
    {
        _service = service;
        _goalService = goalService;
        _goalUi = goalUi;
        _viewInfoUi = viewInfoUi;
        _reportGenerator = reportGenerator;
    }


    public void AddCodingSession(RetrievedCoderDto dto)
    {
        var currentGoal = dto.CurrentCodingGoal;
        if (currentGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding goal available," +
                                            " returning to previous menu\n");
            return;
        }

        if (currentGoal.IsGoalFinished)
        {
            InputHelpers
                .PressAnyKeyToContinue($"Your current coding goal is finished, " +
                                       $"unable to add a new session to the current goal, please add a new goal");
            _viewInfoUi.ViewCurrentCodingGoal(dto);
            _goalUi.AddCodingGoal(dto);
        }

        if (!currentGoal.Sessions.All(s => s.IsSessionFinished))
        {
            InputHelpers
                .PressAnyKeyToContinueError("You have unfinished coding sessions, " +
                                            "please finish any unfinished coding " +
                                            "sessions and then you may add a new session");
            return;
        }
        
        InputHelpers.PressAnyKeyToContinue($"{dto.FirstName}, let's track your coding session");

        var start = GetDate("start time", InputHelpers.DateTimeFormat);

        var endTimeKnown = InputHelpers
            .GetOptionalInput(
                "Do you wish to enter an ending time for your session?\n" +
                "If you are not finished, please come back to update the end time when you are " +
                "finished with your session");

        DateTime? end = null;

        if (endTimeKnown)
        {
            end = GetDate("end time", InputHelpers.DateTimeFormat);
        }

        if (end.HasValue)
        {
            while (!DateTimeHelpers.IsValidEndDate(start, end.Value))
            {
                InputHelpers.PressAnyKeyToContinueError("Start date must come before end date");
                start = GetDate("start time", InputHelpers.DateTimeFormat);
                end = GetDate("end time", InputHelpers.DateTimeFormat);
            }
        }

        var wishToEnterComments = InputHelpers
            .GetOptionalInput("Do you wish to enter comments about this session? ");
        string? comments = null;

        if (wishToEnterComments)
        {
            comments = AnsiConsole.Ask<string>($"[{GetRandomColor()}]Enter comments: [/]").Trim();
        }

        var session = new CreateCodingSessionDto
        {
            GoalId = currentGoal.Id,
            StartTime = start,
            EndTime = end,
            Comments = comments
        };

        if (_service.AddCodingSession(session) != 1)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, " +
                                            $"there was an error logging your coding session" +
                                            $"\nReturning to previous menu");
            return;
        }

        UpdateCodingGoal(dto);
        InputHelpers
            .PressAnyKeyToContinue($"{dto.FirstName}, your coding session has been successfully logged");

        var generateReport =
            InputHelpers.GetOptionalInput(
                $"{dto.FirstName}, would you like to generate a coding report for later viewing? ");

        if (!generateReport) return;
        if (!_reportGenerator.GenerateNewCodingReport(dto))
        {
            InputHelpers.PressAnyKeyToContinueError($"{dto.FirstName}, unfortunately a coding report was not able to be generated");
            return;
        }
        InputHelpers.PressAnyKeyToContinue("You will be able to view your report from the main menu");
    }

    public void UpdateCodingSession(RetrievedCoderDto dto)
    {
        var currentGoal = dto.CurrentCodingGoal;
        if (currentGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding goal available," +
                                            " returning to previous menu\n");
            return;
        }

        if (currentGoal.Sessions.Count == 0)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding sessions to update. Please add a coding session first");
            return;
        }

        if (currentGoal.Sessions.All(s => s.IsSessionFinished))
        {
            InputHelpers
                .PressAnyKeyToContinueError("All of your coding sessions are finished and are not able to be update, " +
                                            "please add a new coding session");
            return;
        }

        if (!_viewInfoUi.ViewCodingSessions(dto))
        {
            return;
        }

        var id = AnsiConsole.Ask<int>($"[{GetRandomColor()}]" +
                                      $"Enter the id of the coding session to update: [/]");
        var sessionToUpdate = _service.GetCodingSession(currentGoal.Id, id);

        if (sessionToUpdate is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, " +
                                            $"error retrieving the coding session to update, nothing updated, " +
                                            $"returning to previous menu");
            return;
        }

        DateTime? end = null;

        if (!sessionToUpdate.EndTime.HasValue)
        {
            end = GetDate("ending time", InputHelpers.DateTimeFormat);
            while (!DateTimeHelpers.IsValidEndDate(sessionToUpdate.StartTime, end.Value))
            {
                InputHelpers
                    .PressAnyKeyToContinueError($"Invalid ending date, must come after " +
                                                $"the start date which is: " +
                                                $"{sessionToUpdate.StartTime.ToString(InputHelpers.DateTimeFormat)}");
                end = GetDate("ending time", InputHelpers.DateTimeFormat);
            }
        }
        
        var comments = InputHelpers.GetOptionalInput("Do you wish to update your comments about this session? ")
            ? AnsiConsole.Ask<string>($"[{GetRandomColor()}]Enter updated comments: [/]").Trim()
            : sessionToUpdate.Comments;

        var wishToUpdateSession = InputHelpers
            .GetOptionalInput($"Are you sure you wish to update session #{id}? ");

        if (!wishToUpdateSession)
        {
            InputHelpers
                .PressAnyKeyToContinue($"{dto.FirstName}, returning to previous menu, nothing updated: ");
            return;
        }

        var updatedSession = new UpdateCodingSessionDto
        {
            Id = sessionToUpdate.Id,
            GoalId = sessionToUpdate.GoalId,
            StartTime = sessionToUpdate.StartTime,
            EndTime = end,
            SessionDuration = sessionToUpdate.SessionDuration,
            Comments = comments,
            IsSessionFinished = sessionToUpdate.IsSessionFinished
        };

        if (_service.UpdateCodingSession(updatedSession) != 1)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error updating " +
                                            $"session #{id}, returning to previous menu");
            return;
        }
            
        UpdateCodingGoal(dto);
        InputHelpers.PressAnyKeyToContinue($"{dto.FirstName}, coding session #{id} successfully updated!");
            
        var generateReport =
            InputHelpers.GetOptionalInput(
                $"{dto.FirstName}, would you like to generate a coding report for later viewing? ");

        if (!generateReport) return;
        if (!_reportGenerator.GenerateNewCodingReport(dto))
        {
            InputHelpers.PressAnyKeyToContinueError($"{dto.FirstName}, unfortunately a coding report was not able to be generated");
            return;
        }
        InputHelpers.PressAnyKeyToContinue("You will be able to view your report from the main menu");

    }

    public void DeleteCodingSession(RetrievedCoderDto dto)
    {
        var currentGoal = dto.CurrentCodingGoal;
        if (currentGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding goal available, returning to previous menu\n");
            return;
        }

        if (currentGoal.Sessions.Count == 0)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding sessions available to delete");
            return;
        }

        if (!_viewInfoUi.ViewCodingSessions(dto))
        {
            return;
        }

        var id = AnsiConsole
            .Ask<int>($"[{GetRandomColor()}]Enter the id of the session to delete: [/]");

        var wishToDeleteSession = InputHelpers
            .GetOptionalInput($"Are you sure you wish to delete session #{id}? ");

        if (!wishToDeleteSession)
        {
            InputHelpers.PressAnyKeyToContinue($"{dto.FirstName}, coding session #{id} not deleted");
        }

        if (_service.DeleteCodingSession(currentGoal.Id, id) != 1)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error deleting coding session #{id} ");
            return;
        }
        
        UpdateCodingGoal(dto);
        InputHelpers.PressAnyKeyToContinue($"{dto.FirstName}, coding session #{id} successfully deleted");
        
        var generateReport =
            InputHelpers.GetOptionalInput(
                $"{dto.FirstName}, would you like to generate a coding report for later viewing? ");

        if (!generateReport) return;
        if (!_reportGenerator.GenerateNewCodingReport(dto))
        {
            InputHelpers.PressAnyKeyToContinueError($"{dto.FirstName}, unfortunately a coding report was not able to be generated");
            return;
        }
        InputHelpers.PressAnyKeyToContinue("You will be able to view your report from the main menu");
    }

    private static DateTime GetDate(string range, string dateFormat)
    {
        DateTime date;
        bool correctChoice;

        do
        {
            var dateString = AnsiConsole
                .Ask<string>($"[{GetRandomColor()}]Please enter {range} " +
                             $"in format: {dateFormat} (24 Hour-Format: 0-23 for the hour) " +
                             $"\n(Date cannot be in the past): [/]").Trim();

            while (!DateTime.TryParseExact(
                       dateString,
                       dateFormat,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out date))
            {
                dateString = AnsiConsole
                    .Ask<string>($"[{GetRandomColor()}]Please enter {range} " +
                                 $"in format: {dateFormat} (24 Hour-Format: 0-23 for the hour) " +
                                 $"\n(Date cannot be in the past): [/]").Trim();
            }

            correctChoice =
                InputHelpers.GetOptionalInput(
                    $"The {range} date you entered is {date.ToString(dateFormat)}, is this correct?");
        } while (!DateTimeHelpers.IsValidDate(date) || !correctChoice);

        return date;
    }

    private void UpdateCodingGoal(RetrievedCoderDto dto)
    {
        if (dto.CurrentCodingGoal is null) return;

        dto.CurrentCodingGoal.Sessions = _service.GetCodingSessions(dto.CurrentCodingGoal.Id);
        dto.CurrentCodingGoal.HoursCodedSoFar = dto.CurrentCodingGoal.GetHoursCodedSoFar();
        dto.CurrentCodingGoal.IsGoalMet = dto.CurrentCodingGoal.GetIsGoalMet();
        dto.CurrentCodingGoal.IsEndDateExpired = dto.CurrentCodingGoal.GetIsEndDateExpired();
        dto.CurrentCodingGoal.IsGoalFinished = dto.CurrentCodingGoal.GetIsGoalFinished();
        var updatedGoal = dto.CurrentCodingGoal.FromRetrievedCodingGoalDtoToUpdateCodingGoalDto();
        _goalService.UpdateCodingGoal(updatedGoal);
    }
}