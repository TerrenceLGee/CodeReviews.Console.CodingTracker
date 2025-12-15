using System.ComponentModel.DataAnnotations;

namespace CodingTracker.TerrenceLGee.TrackerUi.Menus;

public enum SessionMenu
{
    [Display(Name = "Add a coding session to track")]
    AddCodingSession,
    [Display(Name = "Update an already tracked coding session")]
    UpdateCodingSession,
    [Display(Name = "Delete an existing coding session")]
    DeleteCodingSession,
    [Display(Name = "View a coding session")]
    ViewCodingSession,
    [Display(Name = "View all current coding sessions")]
    ViewCodingSessions,
    [Display(Name = "View a previous coding goal")]
    ViewPreviousCodingGoal,
    [Display(Name = "Generate a coding report")]
    GenerateCodingReport,
    [Display(Name = "View lastest coding report")]
    ViewLatestCodingReport,
    [Display(Name = "View any available coding report")]
    ViewAnyCodingReport,
    [Display(Name = "View all available coding reports")]
    ViewAllCodingReports,
    [Display(Name = "Exit the program")]
    Exit,
}