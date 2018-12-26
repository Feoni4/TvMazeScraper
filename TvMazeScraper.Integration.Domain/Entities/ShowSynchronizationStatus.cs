using System.Collections.Generic;

namespace TvMazeScraper.Integration.Domain.Entities
{
    public class ShowSynchronizationStatus
    {
        public int Date { get; set; }

        public List<int> ShowIds { get; set; }

        public ShowSynchronizationStatus()
        {
            ShowIds = new List<int>();
        }
    }
}