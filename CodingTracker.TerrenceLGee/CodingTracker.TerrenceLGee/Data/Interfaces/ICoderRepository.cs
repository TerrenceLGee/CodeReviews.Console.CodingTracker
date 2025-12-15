using CodingTracker.TerrenceLGee.Models;

namespace CodingTracker.TerrenceLGee.Data.Interfaces;

public interface ICoderRepository
{
    int AddCoder(Coder coder);
    Coder? GetCoder(string firstName, string lastName);
    bool CoderAlreadyExists(string firstName, string lastName);
}