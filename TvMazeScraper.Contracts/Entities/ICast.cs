using System;

namespace TvMazeScraper.Contracts.Entities
{
    public interface ICast
    {
        int Id { get; }
        string Name { get; }
        DateTime? Birthday { get; }
    }
}