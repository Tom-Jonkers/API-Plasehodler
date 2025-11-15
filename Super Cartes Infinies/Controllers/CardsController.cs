using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace Super_Cartes_Infinies.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.Include(p => p.CardPowers).ToListAsync());
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Attack,Health,Cost,ImageUrl")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }
        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.Cards
                .Include(c => c.CardPowers)
                .ThenInclude(cp => cp.Power)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null) return NotFound();

            // Get all power IDs already assigned to this card
            var assignedPowerIds = card.CardPowers.Select(cp => cp.PowerId).ToList();

            // Get all powers NOT in the assigned list with formatted text
            var availablePowers = await _context.Powers
                .Where(p => !assignedPowerIds.Contains(p.Id))
                .Select(p => new {
                    p.Id,
                    DisplayText = $"{p.Icone} {p.Name}"  // Combine icon and name
                })
                .ToListAsync();

            ViewBag.Powers = new SelectList(availablePowers, "Id", "DisplayText");
            ViewBag.CardId = card.Id;
            return View(card);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Attack,Health,Cost,Rarete,ImageUrl")] Card card)
        {
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Cards/AddToStartingCards/5
        public async Task<IActionResult> AddToStartingCards(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/AddToStartingCardsConfirmed/5
        [HttpPost, ActionName("AddToStartingCards")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToStartingCardsConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            var startingCard = new StartingCard
            {
                CardId = card.Id,
                Card = card
            };

            _context.StartingCards.Add(startingCard);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "StartingCards");
        }

        public async Task<IActionResult> DeletePower(int id)
        {

           
            var powercard = _context.CardPowers.Where(p => p.Id == id).SingleOrDefault();

            if (powercard != null)
            {
                _context.CardPowers.Remove(powercard);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPower(int cardId, int powerId, int value)
        {
            try
            {
                // Basic validation
                var cardExists = await _context.Cards.AnyAsync(c => c.Id == cardId);
                var powerExists = await _context.Powers.AnyAsync(p => p.Id == powerId);

                if (!cardExists || !powerExists)
                {
                    ModelState.AddModelError("", "Invalid card or power selected");
                    return await AddPower(cardId); // Return to form with error
                }

                // Check for duplicate
                var powerAlreadyAssigned = await _context.CardPowers
                    .AnyAsync(cp => cp.CardId == cardId && cp.PowerId == powerId);

                if (powerAlreadyAssigned)
                {
                    ModelState.AddModelError("", "This power is already assigned to the card");
                    return await AddPower(cardId); // Return to form with error
                }

                // Create and save
                var cardPower = new CardPower
                {
                    CardId = cardId,
                    PowerId = powerId,
                    Value = value
                };

                _context.CardPowers.Add(cardPower);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return await AddPower(cardId); // Return to form with error
            }
        }
        // GET: Cards/AddPower
        public async Task<IActionResult> AddPower(int cardId)
        {
            try
            {
                // Verify card exists
                var card = await _context.Cards.FindAsync(cardId);
                if (card == null)
                {
                    return NotFound();
                }

                // Get available powers
                var assignedPowerIds = await _context.CardPowers
                    .Where(cp => cp.CardId == cardId)
                    .Select(cp => cp.PowerId)
                    .ToListAsync();

                var availablePowers = await _context.Powers
                    .Where(p => !assignedPowerIds.Contains(p.Id))
                    .Select(p => new {
                        p.Id,
                        DisplayText = $"{p.Icone} {p.Name}"
                    })
                    .ToListAsync();

                ViewBag.Powers = new SelectList(availablePowers, "Id", "DisplayText");
                ViewBag.CardId = cardId;

                return View();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
