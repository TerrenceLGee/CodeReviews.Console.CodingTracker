using CodingTracker.TerrenceLGee.DTOs.CoderDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingGoalDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingReportDTOs;
using CodingTracker.TerrenceLGee.DTOs.CodingSessionDTOs;
using CodingTracker.TerrenceLGee.Services.Interfaces;
using CodingTracker.TerrenceLGee.TrackerUi.Helpers;
using CodingTracker.TerrenceLGee.TrackerUi.Interfaces;
using Spectre.Console;
using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi;

public class ViewInfoUi : IViewInfoUi
{
    private readonly ICodingGoalService _goalService;
    private readonly ICodingSessionService _codingSessionService;
    private readonly ICodingReportService _codingReportService;

    public ViewInfoUi(
        ICodingGoalService goalService,
        ICodingSessionService codingSessionService,
        ICodingReportService codingReportService)
    {
        _goalService = goalService;
        _codingSessionService = codingSessionService;
        _codingReportService = codingReportService;
    }

    public void ViewCurrentCodingGoal(RetrievedCoderDto dto)
    {
        dto.CurrentCodingGoal = _goalService.GetCurrentCodingGoal(dto.Id);
        var currentGoal = dto.CurrentCodingGoal;
        if (currentGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("There was an error retrieving your current coding goal");
            return;
        }
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]{dto.FirstName}, here is goal you are currently working towards:\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Began: {currentGoal.StartDate.ToString(InputHelpers.DateFormat)}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Ends: {currentGoal.EndDate.ToString(InputHelpers.DateFormat)}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Actual end date: {currentGoal.ActualEndDate?.ToString(InputHelpers.DateFormat) ?? "N/A"}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Coding goal in hours: {currentGoal.GoalHours}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Total hours to code each day to reach goal: {currentGoal.HoursNeededToReachGoal}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Total hours coded so far: {currentGoal.HoursCodedSoFar}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Is this the current coding goal?: {currentGoal.IsCurrentCodingGoal}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Has this goal expired?: {currentGoal.IsEndDateExpired}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Has this goal been met yet?: {currentGoal.IsGoalMet}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Is this goal finished?: {currentGoal.IsGoalFinished}\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Here are all coding sessions associated with this goal[/]");

        ViewCodingSessions(dto);
    }

    public void ViewPreviousCodingGoal(RetrievedCoderDto dto)
    {
        if (!ViewPreviousCodingGoals(dto))
        {
            return;
        }

        var id = AnsiConsole.Ask<int>($"[{GetRandomColor()}]Enter the id of the previous coding goal to view: [/]");
        var goal = _goalService.GetCodingGoal(dto.Id, id);

        if (goal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"There was an error retrieving coding goal #{id}");
            return;
        }
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]{dto.FirstName}, here is goal #{id}:\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Began: {goal.StartDate.ToString(InputHelpers.DateFormat)}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Ends: {goal.EndDate.ToString(InputHelpers.DateFormat)}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Actual end date: {goal.ActualEndDate?.ToString(InputHelpers.DateFormat) ?? "N/A"}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Coding goal in hours: {goal.GoalHours}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Total hours to code each day to reach goal: {goal.HoursNeededToReachGoal}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Total hours coded so far: {goal.HoursCodedSoFar}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Is this the current coding goal?: {goal.IsCurrentCodingGoal}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Has this goal expired?: {goal.IsEndDateExpired}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Has this goal been met yet?: {goal.IsGoalMet}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Is this goal finished?: {goal.IsGoalFinished}\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Here are all coding sessions associated with this goal[/]");

        var previousSessions = _codingSessionService.GetCodingSessions(goal.Id);

        InputHelpers.ShowPaginatedItems(previousSessions, "coding sessions", DisplayCodingSessions);
    }

    public bool ViewPreviousCodingGoals(RetrievedCoderDto dto)
    {
        var previousGoals = _goalService.GetCodingGoals(dto.Id)
            .Where(g => !g.IsCurrentCodingGoal)
            .ToList();

        return InputHelpers.ShowPaginatedItems(previousGoals, "previous coding goals", DisplayCodingGoals);
    }

    public void ViewLatestCodingReport(RetrievedCoderDto dto)
    {
        dto.Reports = _codingReportService.GetCodingReports(dto.Id);

        if (dto.Reports.Count == 0)
        {
            InputHelpers
                .PressAnyKeyToContinue($"{dto.FirstName}, there are no coding reports available at this time");
            return;
        }
        var latestReport = dto.Reports.LastOrDefault();

        if (latestReport is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error retrieving the latest coding report");
            return;
        }
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Latest coding report for {dto.FirstName}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Report generated on: " +
                               $"{latestReport.ReportGenerated.ToString(InputHelpers.DateTimeFormat)}\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]This coding report contains a summary of all of your coding goals" +
                               $" and the coding sessions associated with those goals (finished and unfinished)\n[/]");
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Days set to complete all coding goals[/]")
            .CenterLabel()
            .AddItem("Total days you have set for all goals", latestReport.TargetDaysSetForGoal, Color.Aqua)
            .AddItem("Total days so far that it took/takes you to reach all goals", latestReport.TotalDaysSetForGoal, Color.BlueViolet));
        
        AnsiConsole.WriteLine();
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Hours you wanted to code combined for all goals[/]")
            .CenterLabel()
            .AddItem("Total goal hours you wanted to code combined for all goals", latestReport.TotalHoursWantingToCode, Color.Cyan)
            .AddItem("Total hours actually coded for combined for all goals so far", latestReport.TotalHoursActuallyCoded, Color.Teal)
            .AddItem("Total hours you needed to code combined for all goals so far", latestReport.TotalHoursNeededToCodeToReachGoal, Color.DarkGoldenrod));

        AnsiConsole.WriteLine();
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Goals met[/]")
            .CenterLabel()
            .AddItem("Total goals you have set so far", latestReport.TotalGoals, Color.SpringGreen3)
            .AddItem("Total goals met so far:", latestReport.HowManyGoalMet, Color.Purple3)
            .AddItem("Total not met before end date", latestReport.HowManyEndDateExpired, Color.CadetBlue));

        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Session information for all goals[/]")
            .CenterLabel()
            .AddItem("Total sessions for all goals", latestReport.TotalSessions, Color.Aquamarine1_1)
            .AddItem("Total finished sessions for all goals", latestReport.NumberOfFinishedSessions, Color.Orchid2));

        var totalSessions =
            $"{latestReport.TotalSessionsDuration.Hours} hours {latestReport.TotalSessionsDuration.Minutes} minutes";
        
        AnsiConsole.MarkupLine($"\n[{GetRandomColor()}]" +
                               $"Total duration of all finished sessions for all goals: " +
                               $"{totalSessions}[/]");
        
        InputHelpers.PressAnyKeyToContinue("\nFinished viewing most recent coding report");
    }

    public bool ViewCodingReports(RetrievedCoderDto dto)
    {
        dto.Reports = _codingReportService.GetCodingReports(dto.Id);
        var reports = dto.Reports;

        return InputHelpers.ShowPaginatedItems(reports, "coding reports", DisplayCodingReports);
    }

    public void ViewAnyCodingReport(RetrievedCoderDto dto)
    {
        dto.Reports = _codingReportService.GetCodingReports(dto.Id);

        if (!ViewCodingReports(dto))
        {
            return;
        }

        var id = AnsiConsole.Ask<int>($"Enter the id of the coding report to view: ");

        var reportToView = _codingReportService.GetCodingReport(dto.Id, id);

        if (reportToView is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error retrieving coding report #{id}");
            return;
        }
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Coding report #{id} for {dto.FirstName}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Report generated on: " +
                               $"{reportToView.ReportGenerated.ToString(InputHelpers.DateTimeFormat)}\n[/]");
        
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]This coding report contains a summary of all of your coding goals" +
                               $" and the coding sessions associated with those goals\n[/]");
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Days set to complete all coding goals[/]")
            .CenterLabel()
            .AddItem("Total days you have set for all goals", reportToView.TargetDaysSetForGoal, Color.Aqua)
            .AddItem("Total days so far that it took/takes you to reach all goals", reportToView.TotalDaysSetForGoal, Color.BlueViolet));
        
        AnsiConsole.WriteLine();
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Hours you wanted to code combined for all goals[/]")
            .CenterLabel()
            .AddItem("Total goal hours you wanted to code combined for all goals", reportToView.TotalHoursWantingToCode, Color.Cyan)
            .AddItem("Total hours actually coded for combined for all goals so far", reportToView.TotalHoursActuallyCoded, Color.Teal)
            .AddItem("Total hours you needed to code combined for all goals so far", reportToView.TotalHoursNeededToCodeToReachGoal, Color.DarkGoldenrod));

        AnsiConsole.WriteLine();
        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Goals met[/]")
            .CenterLabel()
            .AddItem("Total goals you have set so far", reportToView.TotalGoals, Color.SpringGreen3)
            .AddItem("Total goals met so far:", reportToView.HowManyGoalMet, Color.Purple3)
            .AddItem("Total not met before end date", reportToView.HowManyEndDateExpired, Color.CadetBlue));

        
        AnsiConsole.Write(new BarChart()
            .Width(60)
            .Label($"[{GetRandomColor()}]Session information for all goals[/]")
            .CenterLabel()
            .AddItem("Total sessions for all goals", reportToView.TotalSessions, Color.Aquamarine1_1)
            .AddItem("Total finished sessions for all goals", reportToView.NumberOfFinishedSessions, Color.Orchid2));

        var totalSessions =
            $"{reportToView.TotalSessionsDuration.Hours} hours {reportToView.TotalSessionsDuration.Minutes} minutes";
        
        AnsiConsole.MarkupLine($"\n[{GetRandomColor()}]" +
                               $"Total duration of all finished sessions for all goals: " +
                               $"{totalSessions}[/]");
        
        InputHelpers.PressAnyKeyToContinue($"\nFinished viewing coding report #{id}");
    }

    public void ViewCodingSession(RetrievedCoderDto dto)
    {
        dto.CurrentCodingGoal = _goalService.GetCurrentCodingGoal(dto.Id);
        var currentGoal = dto.CurrentCodingGoal;
        if (currentGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError("You currently have no coding goal available, returning to previous menu\n");
            return;
        }

        if (!ViewCodingSessions(dto))
        {
            return;
        }

        var id = AnsiConsole.Ask<int>($"[{GetRandomColor()}]Enter the id of the session to view: [/]");

        var sessionToView = _codingSessionService.GetCodingSession(currentGoal.Id, id);

        if (sessionToView is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError($"{dto.FirstName}, there was an error retrieving coding session #{id}");
            return;
        }
                
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Start time: {sessionToView.StartTime.ToString(InputHelpers.DateTimeFormat)}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"End time: {sessionToView.EndTime?.ToString(InputHelpers.DateTimeFormat) ?? "N/A"}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                               $"Session duration: {sessionToView.SessionDuration?.ToString() ?? "N/A"}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Comments: {sessionToView.Comments ?? "N/A"}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Is session finished? {sessionToView.IsSessionFinished}[/]");
        
        InputHelpers.PressAnyKeyToContinue("\n");
        
    }

    public bool ViewCodingSessions(RetrievedCoderDto dto)
    {
        dto.CurrentCodingGoal = _goalService.GetCurrentCodingGoal(dto.Id);

        if (dto.CurrentCodingGoal is null)
        {
            InputHelpers
                .PressAnyKeyToContinueError(
                    "You currently have no coding goal available, returning to previous menu\n");
            return false;
        }

        dto.CurrentCodingGoal.Sessions = _codingSessionService.GetCodingSessions(dto.CurrentCodingGoal.Id);
        var sessions = dto.CurrentCodingGoal.Sessions;

        return InputHelpers.ShowPaginatedItems(sessions, "coding sessions", DisplayCodingSessions);
    }

    private void DisplayCodingGoals(List<RetrievedCodingGoalDto> goals)
    {
        AnsiConsole.WriteLine();

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Starting date");
        table.AddColumn("Ending date");
        table.AddColumn("Actual ending date");
        table.AddColumn("Goal met?");

        foreach (var goal in goals)
        {
            table.AddRow(
                goal.Id.ToString(),
                goal.StartDate.ToString(InputHelpers.DateFormat),
                goal.EndDate.ToString(InputHelpers.DateFormat),
                goal.EndDate.ToString(InputHelpers.DateFormat),
                goal.IsGoalMet.ToString());
        }
        
        AnsiConsole.Write(table);
    }

    private void DisplayCodingSessions(List<RetrievedCodingSessionDto> sessions) 
    {
        AnsiConsole.WriteLine();
        
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Goal Id");
        table.AddColumn("Start time");
        table.AddColumn("End time");
        table.AddColumn("Session duration");
        table.AddColumn("Comments");
        table.AddColumn("Is session finished?");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.GoalId.ToString(),
                session.StartTime.ToString(InputHelpers.DateTimeFormat),
                session.EndTime?.ToString(InputHelpers.DateTimeFormat) ?? "N/A", 
                session.SessionDuration.HasValue ? session.SessionDuration.Value.ToString() : "N/A",
                session.Comments ?? "N/A", 
                session.IsSessionFinished.ToString());
        }
        
        AnsiConsole.Write(table);
    }

    private void DisplayCodingReports(List<RetrievedCodingReportDto> reports)
    {
        AnsiConsole.WriteLine();

        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Targeted total days set for all goals");
        table.AddColumn("Total days it took for goals");
        table.AddColumn("Targeted total hours wanting to code for all goals");
        table.AddColumn("Total hours needed to code to reach goals");
        table.AddColumn("Total hours actually coded for all goals");
        table.AddColumn("Total end date expired before goals met");
        table.AddColumn("Total in which goals were met");
        table.AddColumn("Total duration of all coding sessions for all goals");
        table.AddColumn("Total sessions finished for all goals");
        table.AddColumn("Total goals");
        table.AddColumn("Total sessions");
        table.AddColumn("Report generated");

        foreach (var report in reports)
        {
            table.AddRow(
                report.Id.ToString(),
                report.TargetDaysSetForGoal.ToString(),
                report.TotalDaysSetForGoal.ToString(),
                report.TotalHoursWantingToCode.ToString(),
                report.TotalHoursNeededToCodeToReachGoal.ToString(),
                report.TotalHoursActuallyCoded.ToString(),
                report.HowManyEndDateExpired.ToString(),
                report.HowManyGoalMet.ToString(),
                report.TotalSessionsDuration.ToString(),
                report.NumberOfFinishedSessions.ToString(),
                report.TotalGoals.ToString(),
                report.TotalSessions.ToString(),
                report.ReportGenerated.ToString(InputHelpers.DateTimeFormat));
        }
        AnsiConsole.Write(table);
    }
}