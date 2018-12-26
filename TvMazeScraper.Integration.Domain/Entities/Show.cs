using System.Collections.Generic;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class Show : IShow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ICast> Cast { get; set; }
    }
}