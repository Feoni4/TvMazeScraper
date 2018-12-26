using System.Collections.Generic;

namespace TvMazeScraper.Contracts.Entities
{
    public interface IShow
    {
        int Id { get; }
        string Name { get; }

        List<ICast> Cast { get; }
    }
}