using System;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class Cast : ICast
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }
    }
}