namespace CodingTracker.TerrenceLGee.Models;

public class Coder
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CodingGoal? CurrentGoal { get; set; }
    public List<CodingGoal> Goals { get; set; } = [];
    public List<CodingReport> Reports { get; set; } = [];
}