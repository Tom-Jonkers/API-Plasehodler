using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using System;

namespace Super_Cartes_Infinies.Services
{
    public class DecksService
    {
        private ApplicationDbContext _dbContext;

        public DecksService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        private async Task<GameConfig> GetGameConfig()
        {
            return await _dbContext.GameConfigs.FirstOrDefaultAsync() 
                ?? new GameConfig { maxDecks = 5, maxCardsPerDeck = 30 }; // Valeurs par défaut si aucune config n'existe
        }

        public async Task<IEnumerable<Deck>> GetPlayerDecks(Player player)
        {
            return await _dbContext.Decks.Where(d => d.Player == player)
                .Include(d => d.OwnedCards)
                    .ThenInclude(oc => oc.Card)
                .ToListAsync();
        }

        public async Task<Deck> CreateDeck(string name, Player player)
        {
            // Vérifier la limite de decks
            var config = await GetGameConfig();
            var playerDeckCount = await _dbContext.Decks.CountAsync(d => d.PlayerId == player.Id);
            
            if (playerDeckCount >= config.maxDecks)
            {
                throw new LimitExceededException($"Vous avez atteint la limite maximale de {config.maxDecks} decks.");
            }
            
            var deck = new Deck
            {
                Name = name,
                Player = player,
                nbVictoires = 0,
                nbDefaites = 0
            };
            
            _dbContext.Decks.Add(deck);
            await _dbContext.SaveChangesAsync();
            return deck;
        }

        public async Task<bool> DeleteDeck(int deckId)
        {
            var deck = await _dbContext.Decks.FindAsync(deckId);
            if (deck == null)
            {
                return false;
            }
            
            // Vérifier que le deck n'est pas défini comme courant
            if (deck.IsCurrent)
            {
                return false; // Impossible de supprimer un deck courant
            }

            _dbContext.Decks.Remove(deck);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Deck> GetDeckById(int deckId)
        {
            return await _dbContext.Decks
                .Include(d => d.Player)
                .FirstOrDefaultAsync(d => d.Id == deckId);
        }

        public async Task<bool> SetCurrentDeck(int deckId, Player player)
        {
            // Trouver tous les decks du joueur
            var playerDecks = await _dbContext.Decks
                .Where(d => d.PlayerId == player.Id)
                .ToListAsync();

            // Mettre IsCurrent à false pour tous les decks du joueur
            foreach (var deck in playerDecks)
            {
                deck.IsCurrent = false;
            }

            // Trouver le nouveau deck courant
            var newCurrentDeck = playerDecks.FirstOrDefault(d => d.Id == deckId);
            if (newCurrentDeck == null)
            {
                return false;
            }

            // Mettre IsCurrent à true pour le nouveau deck courant
            newCurrentDeck.IsCurrent = true;

            // Sauvegarder les changements
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCardToDeck(int ownedCardId, int deckId, Player player, string userId)
        {
            // Vérifier que le deck appartient au joueur
            var deck = await _dbContext.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deckId && d.PlayerId == player.Id);

            if (deck == null)
            {
                return false;
            }

            // Vérifier que le joueur possède la carte
            var ownedCard = await _dbContext.OwnedCards
                .FirstOrDefaultAsync(oc => oc.Id == ownedCardId && oc.PlayerId == userId);

            if (ownedCard == null)
            {
                return false;
            }

            // Vérifier si la carte est déjà dans le deck
            if (deck.OwnedCards.Any(oc => oc.Id == ownedCard.Id))
            {
                return true; // La carte est déjà dans le deck, considéré comme succès
            }

            // Vérifier la limite de cartes par deck
            var config = await GetGameConfig();
            if (deck.OwnedCards.Count >= config.maxCardsPerDeck)
            {
                throw new LimitExceededException($"Le deck a atteint la limite maximale de {config.maxCardsPerDeck} cartes.");
            }

            // Ajouter la carte au deck
            deck.OwnedCards.Add(ownedCard);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCardFromDeck(int ownedCardId, int deckId, Player player, string userId)
        {
            // Vérifier que le deck appartient au joueur
            var deck = await _dbContext.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deckId && d.PlayerId == player.Id);

            if (deck == null)
            {
                return false;
            }

            // Vérifier que le joueur possède la carte
            var ownedCard = await _dbContext.OwnedCards
                .FirstOrDefaultAsync(oc => oc.Id == ownedCardId && oc.PlayerId == userId);

            if (ownedCard == null)
            {
                return false;
            }

            // Vérifier si la carte est dans le deck
            var cardInDeck = deck.OwnedCards.FirstOrDefault(oc => oc.Id == ownedCardId);
            if (cardInDeck == null)
            {
                return false; // La carte n'est pas dans le deck
            }

            // Retirer la carte du deck
            deck.OwnedCards.Remove(cardInDeck);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Card>> GetCurrentDeckCards(int playerId)
        {
            var currentDeck = await _dbContext.Decks
                .Where(d => d.PlayerId == playerId && d.IsCurrent)
                .Include(d => d.OwnedCards)
                    .ThenInclude(oc => oc.Card)
                .FirstOrDefaultAsync();

            if (currentDeck == null)
            {
                return new List<Card>();
            }

            return currentDeck.OwnedCards.Select(oc => oc.Card);
        }
    }

    public class LimitExceededException : Exception
    {
        public LimitExceededException(string message) : base(message)
        {
        }
    }
}

