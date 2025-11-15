using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Controllers
{
    public class GameConfigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameConfigsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GameConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.GameConfigs.ToListAsync());
        }

        // GET: GameConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameConfig = await _context.GameConfigs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameConfig == null)
            {
                return NotFound();
            }

            return View(gameConfig);
        }

        // GET: GameConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,nbCardToDraw,qtManaPerTurn,MonnaieRecueVictoire,MonnaieRecueDefaite,MonnaieRecueCreation,maxDecks,maxCardsPerDeck")] GameConfig gameConfig)

        {
            if (ModelState.IsValid)
            {
                _context.Add(gameConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameConfig);
        }

        // GET: GameConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameConfig = await _context.GameConfigs.FindAsync(id);
            if (gameConfig == null)
            {
                return NotFound();
            }
            return View(gameConfig);
        }

        // POST: GameConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,nbCardToDraw,qtManaPerTurn,MonnaieRecueVictoire,MonnaieRecueDefaite,MonnaieRecueCreation,maxDecks,maxCardsPerDeck")] GameConfig gameConfig)

        {
            if (id != gameConfig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameConfigExists(gameConfig.Id))
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
            return View(gameConfig);
        }

        // GET: GameConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameConfig = await _context.GameConfigs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameConfig == null)
            {
                return NotFound();
            }

            return View(gameConfig);
        }

        // POST: GameConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameConfig = await _context.GameConfigs.FindAsync(id);
            if (gameConfig != null)
            {
                _context.GameConfigs.Remove(gameConfig);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameConfigExists(int id)
        {
            return _context.GameConfigs.Any(e => e.Id == id);
        }
    }
}
