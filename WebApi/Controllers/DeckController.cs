using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Models.Dtos;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using System.Security.Claims;

namespace Super_Cartes_Infinies.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private DecksService _decksService;
        private PlayersService _playersService;
        private readonly UserManager<IdentityUser> _userManager;

        public DeckController(ApplicationDbContext dbContext, DecksService decksService, PlayersService playersService, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _decksService = decksService;
            _playersService = playersService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetPlayerDecks()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            var decks = await _decksService.GetPlayerDecks(player);

            return Ok(decks);
        }

        [HttpPost]
        public async Task<ActionResult<Deck>> CreateDeck([FromBody] CreateDeckDTO createDeckDTO)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            try
            {
                var createdDeck = await _decksService.CreateDeck(createDeckDTO.Name, player);
                return CreatedAtAction(nameof(GetPlayerDecks), new { id = createdDeck.Id }, createdDeck);
            }
            catch (LimitExceededException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            var deck = await _decksService.GetDeckById(id);
            if (deck == null || deck.Player.Id != player.Id)
            {
                return NotFound(new { Error = "Deck not found or you do not have permission to delete this deck." });
            }
            
            // Vérifier si le deck est défini comme courant
            if (deck.IsCurrent)
            {
                return BadRequest(new { Error = "Cannot delete the current deck. Please set another deck as current before deletion." });
            }

            var result = await _decksService.DeleteDeck(id);
            if (!result)
            {
                return StatusCode(500, new { Error = "An error occurred while deleting the deck." });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SetCurrentDeck(int id)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            var deck = await _decksService.GetDeckById(id);
            if (deck == null || deck.Player.Id != player.Id)
            {
                return NotFound(new { Error = "Deck not found or you do not have permission to modify this deck." });
            }

            var result = await _decksService.SetCurrentDeck(id, player);
            if (!result)
            {
                return StatusCode(500, new { Error = "An error occurred while setting the current deck." });
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddCardToDeck([FromBody] DeckCardOperationDTO deckCardOperation)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            try
            {
                var result = await _decksService.AddCardToDeck(deckCardOperation.OwnedCardId, deckCardOperation.DeckId, player, user.Id);
                if (!result)
                {
                    return BadRequest(new { Error = "Failed to add card to deck. Make sure the deck and card belong to the player." });
                }

                return Ok(new { Message = "Card added to deck successfully." });
            }
            catch (LimitExceededException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCardFromDeck([FromBody] DeckCardOperationDTO deckCardOperation)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            var player = _playersService.GetPlayerFromUserId(user.Id);
            if (player == null)
            {
                return NotFound(new { Error = "Player not found." });
            }

            var result = await _decksService.RemoveCardFromDeck(deckCardOperation.OwnedCardId, deckCardOperation.DeckId, player, user.Id);
            if (!result)
            {
                return BadRequest(new { Error = "Failed to remove card from deck. Make sure the deck and card belong to the player and the card is in the deck." });
            }

            return Ok(new { Message = "Card removed from deck successfully." });
        }
    }
}

