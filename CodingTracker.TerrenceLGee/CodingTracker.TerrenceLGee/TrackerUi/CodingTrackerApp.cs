using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.TrackerUi.Helpers;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Menus;
using Spectre.Console;
using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class CodingTrackerApp
{
    private readonly ICoderUi _coderUi;
    private readonly ICodingGoalUi _codingGoalUi;
    private readonly ICodingSessionUi _codingSessionUi;
    private readonly ICodingReportUi _codingReportUi;
    private readonly IViewInfoUi _viewInfoUi;

    public CodingTrackerApp(
        ICoderUi coderUi,
        ICodingGoalUi codingGoalUi,
        ICodingSessionUi codingSessionUi,
        ICodingReportUi codingReportUi,
        IViewInfoUi viewInfoUi)
    {
        _coderUi = coderUi;
        _codingGoalUi = codingGoalUi;
        _codingSessionUi = codingSessionUi;
        _codingReportUi = codingReportUi;
        _viewInfoUi = viewInfoUi;
    }

    public void Run()
    {
        AnsiConsole.Write(
            new FigletText("Welcome to the Coding Tracker\n")
                .Centered()
                .Color(Color.Cyan3));

        InputHelpers.PressAnyKeyToContinue();

        var coder = _coderUi.GetCoder();

        if (coder is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("There has been an unexpected error starting the coding tracker\nExiting");
            return;
        }
        
        _codingGoalUi.Initialize(coder);

        if (coder.CurrentCodingGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("There was an unexpected error initializing your coding goal\nExiting");
            return;
        }
        
        InputHelpers
            .PressAnyKeyToContinue($"{coder.FirstName}, before we begin let's take a " +
                                   $"look at the goal you are currently working towards");
        
        _viewInfoUi.ViewCurrentCodingGoal(coder);

        var coderFinished = false;

        while (!coderFinished)
        {
            var choice = InputHelpers.GetMenuChoice(coder.FirstName);

            switch (choice)
            {
                case SessionMenu.AddCodingSession:
                    _codingSessionUi.AddCodingSession(coder);
                    break;
                case SessionMenu.UpdateCodingSession:
                    _codingSessionUi.UpdateCodingSession(coder);
                    break;
                case SessionMenu.DeleteCodingSession:
                    _codingSessionUi.DeleteCodingSession(coder);
                    break;
                case SessionMenu.ViewCodingSession:
                    _viewInfoUi.ViewCodingSession(coder);
                    break;
                case SessionMenu.ViewCodingSessions:
                    _viewInfoUi.ViewCodingSessions(coder);
                    break;
                case SessionMenu.ViewPreviousCodingGoal:
                    _viewInfoUi.ViewPreviousCodingGoal(coder);
                    break;
                case SessionMenu.GenerateCodingReport:
                    _codingReportUi.GenerateReport(coder);
                    break;
                case SessionMenu.ViewLatestCodingReport:
                    _viewInfoUi.ViewLatestCodingReport(coder);
                    break;
                case SessionMenu.ViewAnyCodingReport:
                    _viewInfoUi.ViewAnyCodingReport(coder);
                    break;
                case SessionMenu.ViewAllCodingReports:
                    _viewInfoUi.ViewCodingReports(coder);
                    break;
                case SessionMenu.Exit:
                    coderFinished = true;
                    break;
                default:
                    InputHelpers.PressAnyKeyToContinueError("Invalid option entered, please try again");
                    break;
            }

            if (coder.CurrentCodingGoal is not null)
            {
                if (!coder.CurrentCodingGoal.IsGoalFinished) continue;
            }
            
            HandleGoalFinished(coder);
            var setNewGoal =
                InputHelpers.GetOptionalInput($"{coder.FirstName}, you don't have a current coding goal" +
                                              $" would you like to set a goal now? ");
            if (setNewGoal)
            {
                _codingGoalUi.AddCodingGoal(coder);
                InputHelpers.PressAnyKeyToContinue($"{coder.FirstName}, let's take a look at your new goal");
                _viewInfoUi.ViewCurrentCodingGoal(coder);
            }
            else
            {
                _codingGoalUi.ResetGoal(coder);
                InputHelpers.PressAnyKeyToContinue($"{coder.FirstName}, " +
                                                   $"remember that you will have to set a new goal " +
                                                   $"before you track another coding session");
            }
        }

        if (coder.CurrentCodingGoal is not null && coder.CurrentCodingGoal.IsCurrentCodingGoal)
        {
            var viewGoal = InputHelpers.GetOptionalInput(
                $"{coder.FirstName}, before you go would " +
                $"you like to look at your current goal and see your progress so far? ");
        
            if (viewGoal && coder.CurrentCodingGoal is not null) _viewInfoUi.ViewCurrentCodingGoal(coder);
        }
        
        InputHelpers.PressAnyKeyToContinue($"{coder.FirstName}, thank you for using the Coding Tracker\nGoodbye");
    }

    private void HandleGoalFinished(RetrievedCoderDto coder)
    {
        if (coder.CurrentCodingGoal is null || !coder.CurrentCodingGoal.IsCurrentCodingGoal) return;
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]{coder.FirstName}, looks like your current coding goal is finished!\n[/]");
        InputHelpers.PressAnyKeyToContinue(coder.CurrentCodingGoal!.IsGoalMet 
            ? "Congratulations, you achieved your goal!\n"
            : "I'm sorry you did not reach your goal this time\n" + 
              "Let's take a look at your goal");
        _viewInfoUi.ViewCurrentCodingGoal(coder);
        coder.CurrentCodingGoal.IsCurrentCodingGoal = false;
    }
}