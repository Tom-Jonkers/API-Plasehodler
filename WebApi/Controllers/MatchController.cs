using Microsoft.AspNetCore.Mvc;
using Models.Models.Dtos;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MatchController : Controller
    {
        private MatchesService _matchService;

        public MatchController(MatchesService matchesService)
        {
            _matchService = matchesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MatchDTO>> GetAllMatches()
        {
            return Ok(_matchService.GetAllMatches());
        }
    }
}
