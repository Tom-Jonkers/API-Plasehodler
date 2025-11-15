using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Super_Cartes_Infinies.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private CardsService _cardsService;
        private readonly UserManager<IdentityUser> _userManager;

        public CardController(ApplicationDbContext dbContext, CardsService cardsService, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _cardsService = cardsService;
		    _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Card>> GetAllCards()
        {
            return Ok(_cardsService.GetAllCards());
        }

        // TODO: La version réelle devra utiliser [Authorize] pour protéger les données est s'assurer d'avoir accès au User
        // Et l'utiliser pour obtenir l'Id de l'utilisateur
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Card>>> GetPlayersCards()
        {
		    var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_cardsService.GetPlayersCards(user.Id));
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Card>>> GetPlayersOwnedCards()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_cardsService.GetPlayersOwnedCards(user.Id));
        }
        
        [HttpGet]
        public IActionResult ApplyMigrations()
        {
            _dbContext.Database.Migrate();
            return Ok("La BD est maintenant à jour!");
        }
    }
}
