using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Presentation.Domain;
using TvMazeScraper.Presentation.Entities;

namespace TvMazeScraper.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SortedShowStore _showStore;

        public ShowController(IMapper mapper, SortedShowStore showStore)
        {
            _mapper = mapper;
            _showStore = showStore;
        }

        [HttpGet("shows")]
        public async Task<ActionResult<IEnumerable<Show>>> GetAsync(int page, int count)
        {
            var offset = (page - 1) * count;
            var rawShowlist = await _showStore.GetAsync(offset, count);

            if (rawShowlist.Count == 0)
            {
                return new NotFoundResult();
            }

            var showList = _mapper.Map<List<Show>>(rawShowlist);

            return showList;
        }

        [HttpGet("show/{id}")]
        public async Task<ActionResult<Show>> GetAsync(int id)
        {
            var rawShow = await _showStore.GetAsync(id);
            if (rawShow == null)
            {
                return new NotFoundResult();
            }

            var show = _mapper.Map<Show>(rawShow);

            return show;
        }
    }
}