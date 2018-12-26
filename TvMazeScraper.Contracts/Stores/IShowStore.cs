using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Contracts.Entities;

namespace TvMazeScraper.Contracts
{
    public interface IShowStore
    {
        Task SetAsync(IShow show);
        Task<IShow> GetAsync(int id);
        Task<List<IShow>> GetAsync(int offset, int count);
        Task RemoveNotMatchedAsync(IEnumerable<int> ids);
    }
}