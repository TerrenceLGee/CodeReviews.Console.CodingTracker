using System.ComponentModel.DataAnnotations;
using CodingTracker.TerrenceLGee.Extensions;
using CodingTracker.TerrenceLGee.TrackerUi.Menus;
using Spectre.Console;

using static CodingTracker.TerrenceLGee.TrackerUi.Helpers.ConsoleHelpers;

namespace CodingTracker.TerrenceLGee.TrackerUi.Helpers;

public static class InputHelpers
{
    public const string DateTimeFormat = "MM-dd-yyyy HH:mm";
    public const string DateFormat = "MM-dd-yyyy";

    public static void PressAnyKeyToContinue(string message = "")
    {
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]{message}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Press any key to continue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    public static void PressAnyKeyToContinueError(string errorMessage)
    {
        AnsiConsole.MarkupLine($"[bold underline red]{errorMessage}[/]");
        AnsiConsole.MarkupLine($"[{GetRandomColor()}]Press any key to cotinue[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    public static bool GetOptionalInput(string message)
    {
        return AnsiConsole.Confirm($"[{GetRandomColor()}]{message}[/]");
    }

    public static bool ShowPaginatedItems<T>(
        List<T> items,
        string name,
        Action<List<T>> display,
        int pageSize = 10)
    {
        if (items.Count == 0)
        {
            PressAnyKeyToContinueError($"You currently have no {name} available to display");
            return false;
        }

        var pageIndex = 0;
        var pageCount = (int)Math.Ceiling(items.Count / (double)pageSize);

        while (true)
        {
            var pagedItems = items
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
            
            AnsiConsole.MarkupLine($"[{GetRandomColor()}]" +
                                   $"Page {pageIndex + 1} of {pageCount} (showing {pagedItems.Count} of " +
                                   $"{items.Count})[/]");

            display(pagedItems);

            var prompt = new SelectionPrompt<Choices>()
                .Title($"[{GetRandomColor()}]Navigate pages: [/]");

            if (pageIndex > 0)
            {
                prompt.AddChoice(Choices.Previous);
            }

            prompt.AddChoice(Choices.Exit);

            if (pageIndex < pageCount - 1)
            {
                prompt.AddChoice(Choices.Next);
            }

            var choice = AnsiConsole.Prompt(prompt);

            if (choice == Choices.Next && pageIndex < pageCount - 1)
            {
                pageIndex++;
            }
            else if (choice == Choices.Previous && pageIndex > 0)
            {
                pageIndex--;
            }
            else
            {
                break;
            }
        }

        PressAnyKeyToContinue();
        
        return true;
    }

    public static SessionMenu GetMenuChoice(string firstName)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<SessionMenu>()
                .Title($"[{GetRandomColor()}]{firstName}, please choose one of the following options[/]")
                .AddChoices(Enum.GetValues<SessionMenu>())
                .UseConverter(choice => choice.GetDisplayName()));
    }
}

public enum Choices
{
    [Display(Name = "Previous Page")]
    Previous,
    [Display(Name = "Next Page")]
    Next,
    [Display(Name = "Leave")]
    Exit
}