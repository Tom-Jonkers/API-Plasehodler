using Microsoft.AspNetCore.Mvc;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StartingCardController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private StartingCardsService _startingCardsService;

        public StartingCardController(ApplicationDbContext dbContext, StartingCardsService startingCardsService)
        {
            _dbContext = dbContext;
            _startingCardsService = startingCardsService;
        }

        [HttpGet]
        public ActionResult<List<Card>> GetStartingCards()
        {
            return Ok(_startingCardsService.GetStartingCards());
        }
    }
}
