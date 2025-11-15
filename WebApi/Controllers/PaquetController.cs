using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace Super_Cartes_Infinies.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaquetController : ControllerBase
    {
        private PaquetsService _paquetsService;
        private CardsService _cardsService;


        public PaquetController(PaquetsService paquetsService, CardsService cardsService)
        {
            _paquetsService = paquetsService;
            _cardsService = cardsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Paquet>> GetAllPaquets()
        {
            return Ok(_paquetsService.GetAllPaquets());
        }

        [HttpPost]
        public ActionResult<IEnumerable<Card>> OpenPack(Paquet paquet)
        {
            var playerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(playerId))
            {
                return Unauthorized("Impossible de récupérer l'ID du joueur. Veuillez vous connecter.");
            }

            var allCards = _cardsService.GetAllCards();
            var result = _paquetsService.OpenPack(paquet, allCards, playerId);
            return Ok(result);
        }
    }
}
