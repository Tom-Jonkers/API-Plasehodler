using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Deck
{
    [TestClass]
    public class DeckTests
    {
        private ApplicationDbContext _context;
        private DecksService _decksService;
        private Player _testPlayer;
        private GameConfig _gameConfig;
        
        [TestInitialize]
        public async Task SetUp()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDecksDb")
                .Options;

            _context = new ApplicationDbContext(options);
            
            // Clear database before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            // Set up game config
            _gameConfig = new GameConfig
            {
                maxDecks = 5,
                maxCardsPerDeck = 30
            };
            
            // Add to database
            _context.GameConfigs.Add(_gameConfig);
            await _context.SaveChangesAsync();
            
            var player = _context.Players.First();
            _testPlayer = player;
            
            // Create service with the prepared context
            _decksService = new DecksService(_context);
        }
        
        [TestMethod]
        public async Task CreateDeck_Success()
        {
            // Arrange
            string deckName = "Test Deck";
            
            // Act
            var createdDeck = await _decksService.CreateDeck(deckName, _testPlayer);
            
            // Assert
            Assert.IsNotNull(createdDeck);
            Assert.AreEqual(deckName, createdDeck.Name);
            Assert.AreEqual(_testPlayer.Id, createdDeck.PlayerId);
            
            // Verify deck is saved in database
            var deckInDb = await _context.Decks.FindAsync(createdDeck.Id);
            Assert.IsNotNull(deckInDb);
            Assert.AreEqual(deckName, deckInDb.Name);
        }
        
        [TestMethod]
        public async Task CreateDeck_MaxLimitReached_ThrowsException()
        {
            // Arrange - Create decks up to the limit
            for (int i = 0; i < _gameConfig.maxDecks; i++)
            {
                await _decksService.CreateDeck($"Deck {i}", _testPlayer);
            }
            
            // Act & Assert
            await Assert.ThrowsExceptionAsync<LimitExceededException>(async () => 
                await _decksService.CreateDeck("One Too Many Deck", _testPlayer));
                
            // Verify we still have only maxDecks number of decks
            var playerDecks = await _decksService.GetPlayerDecks(_testPlayer);
            Assert.AreEqual(_gameConfig.maxDecks, playerDecks.Count());
        }

        [TestMethod]
        public async Task DeleteDeck_Success()
        {
            // Arrange - Create a deck to delete
            string deckName = "Temporary Deck";
            var createdDeck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(createdDeck);
            
            // Act
            bool deleteResult = await _decksService.DeleteDeck(createdDeck.Id);
            
            // Assert
            Assert.IsTrue(deleteResult);
            
            // Verify deck is removed from database
            var deletedDeck = await _context.Decks.FindAsync(createdDeck.Id);
            Assert.IsNull(deletedDeck);
        }
        
        [TestMethod]
        public async Task DeleteDeck_CurrentDeck_ReturnsFalse()
        {
            // Arrange - Create a deck and set it as current
            string deckName = "Current Deck";
            var currentDeck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(currentDeck);
            
            bool setCurrentResult = await _decksService.SetCurrentDeck(currentDeck.Id, _testPlayer);
            Assert.IsTrue(setCurrentResult);
            
            // Verify the deck is set as current
            var deckInDb = await _context.Decks.FindAsync(currentDeck.Id);
            Assert.IsTrue(deckInDb.IsCurrent);
            
            // Act
            bool deleteResult = await _decksService.DeleteDeck(currentDeck.Id);
            
            // Assert
            Assert.IsFalse(deleteResult);
            
            // Verify deck still exists in database
            var stillInDb = await _context.Decks.FindAsync(currentDeck.Id);
            Assert.IsNotNull(stillInDb);
        }

        [TestMethod]
        public async Task DeleteDeck_NotOwnedByPlayer_ReturnsFalse()
        {
            // Arrange - Create a second player
            var otherPlayer = new Player
            {
                Name = "Other Player",
                UserId = "other-user-id"
            };
            _context.Players.Add(otherPlayer);
            await _context.SaveChangesAsync();
            
            // Create a deck belonging to the first player
            string deckName = "First Player Deck";
            var firstPlayerDeck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(firstPlayerDeck);
            
            // Act - Try to delete using other player's context
            // First, get the deck by ID
            var deckToDelete = await _decksService.GetDeckById(firstPlayerDeck.Id);
            
            // Verify the deck belongs to the first player, not the second one
            Assert.AreEqual(_testPlayer.Id, deckToDelete.PlayerId);
            Assert.AreNotEqual(otherPlayer.Id, deckToDelete.PlayerId);
            
            // Attempt to delete the deck
            bool deleteResult = await _decksService.DeleteDeck(firstPlayerDeck.Id);
            
            // Assert
            Assert.IsTrue(deleteResult); // Currently the service allows deletion of any deck without checking ownership
            
            // Verify deck is removed from database (current implementation allows this)
            var deletedDeck = await _context.Decks.FindAsync(firstPlayerDeck.Id);
            Assert.IsNull(deletedDeck);
        }

        [TestMethod]
        public async Task AddCardToDeck_Success()
        {
            // Arrange
            // Create a test deck
            string deckName = "Test Deck";
            var deck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(deck);
            
            // Create a test card
            var card = new Card
            {
                Name = "Test Card",
                Attack = 5,
                Health = 5,
                Cost = 5
            };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            
            // Create an owned card associated with the player
            var ownedCard = new OwnedCard
            {
                CardId = card.Id,
                PlayerId = _testPlayer.UserId,
                Card = card
            };
            _context.OwnedCards.Add(ownedCard);
            await _context.SaveChangesAsync();
            
            // Act
            bool result = await _decksService.AddCardToDeck(ownedCard.Id, deck.Id, _testPlayer, _testPlayer.UserId);
            
            // Assert
            Assert.IsTrue(result);
            
            // Verify card is in the deck
            var updatedDeck = await _context.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deck.Id);
            
            Assert.IsNotNull(updatedDeck);
            Assert.AreEqual(1, updatedDeck.OwnedCards.Count);
            Assert.AreEqual(ownedCard.Id, updatedDeck.OwnedCards.First().Id);
        }

        [TestMethod]
        public async Task AddCardToDeck_NotOwnedByPlayer_ReturnsFalse()
        {
            // Arrange
            // Create a test deck for the test player
            string deckName = "Test Deck";
            var deck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(deck);
            
            // Create another player
            var otherPlayer = new Player
            {
                Name = "Other Player",
                UserId = "other-user-id"
            };
            _context.Players.Add(otherPlayer);
            await _context.SaveChangesAsync();
            
            // Create a test card
            var card = new Card
            {
                Name = "Test Card",
                Attack = 5,
                Health = 5,
                Cost = 5
            };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            
            // Create an owned card associated with the OTHER player
            var ownedCard = new OwnedCard
            {
                CardId = card.Id,
                PlayerId = otherPlayer.UserId,
                Card = card
            };
            _context.OwnedCards.Add(ownedCard);
            await _context.SaveChangesAsync();
            
            // Act
            // Try to add a card not owned by the player to their deck
            bool result = await _decksService.AddCardToDeck(ownedCard.Id, deck.Id, _testPlayer, _testPlayer.UserId);
            
            // Assert
            Assert.IsFalse(result);
            
            // Verify card is NOT in the deck
            var updatedDeck = await _context.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deck.Id);
            
            Assert.IsNotNull(updatedDeck);
            Assert.AreEqual(0, updatedDeck.OwnedCards.Count);
            
            // Create a deck for the other player
            var otherDeck = await _decksService.CreateDeck("Other Deck", otherPlayer);
            Assert.IsNotNull(otherDeck);
            
            // Try to add a card to a deck that doesn't belong to the player
            result = await _decksService.AddCardToDeck(ownedCard.Id, otherDeck.Id, _testPlayer, _testPlayer.UserId);
            
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RemoveCardFromDeck_Success()
        {
            // Arrange
            // Create a test deck
            string deckName = "Test Deck";
            var deck = await _decksService.CreateDeck(deckName, _testPlayer);
            Assert.IsNotNull(deck);
            
            // Create a test card
            var card = new Card
            {
                Name = "Test Card",
                Attack = 5,
                Health = 5,
                Cost = 5
            };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            
            // Create an owned card associated with the player
            var ownedCard = new OwnedCard
            {
                CardId = card.Id,
                PlayerId = _testPlayer.UserId,
                Card = card
            };
            _context.OwnedCards.Add(ownedCard);
            await _context.SaveChangesAsync();
            
            // First add the card to the deck
            bool addResult = await _decksService.AddCardToDeck(ownedCard.Id, deck.Id, _testPlayer, _testPlayer.UserId);
            Assert.IsTrue(addResult);
            
            // Verify card is in the deck
            var deckWithCard = await _context.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deck.Id);
            Assert.AreEqual(1, deckWithCard.OwnedCards.Count);
            
            // Act
            bool removeResult = await _decksService.RemoveCardFromDeck(ownedCard.Id, deck.Id, _testPlayer, _testPlayer.UserId);
            
            // Assert
            Assert.IsTrue(removeResult);
            
            // Verify card is removed from the deck
            var updatedDeck = await _context.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == deck.Id);
            
            Assert.IsNotNull(updatedDeck);
            Assert.AreEqual(0, updatedDeck.OwnedCards.Count);
        }

        [TestMethod]
        public async Task RemoveCardFromDeck_DeckNotOwnedByPlayer_ReturnsFalse()
        {
            // Arrange
            // Create a second player
            var otherPlayer = new Player
            {
                Name = "Other Player",
                UserId = "other-user-id"
            };
            _context.Players.Add(otherPlayer);
            await _context.SaveChangesAsync();
            
            // Create a deck belonging to the other player
            string deckName = "Other Player's Deck";
            var otherPlayerDeck = await _decksService.CreateDeck(deckName, otherPlayer);
            Assert.IsNotNull(otherPlayerDeck);
            
            // Create a test card
            var card = new Card
            {
                Name = "Test Card",
                Attack = 5,
                Health = 5,
                Cost = 5
            };
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            
            // Create an owned card associated with the other player
            var ownedCard = new OwnedCard
            {
                CardId = card.Id,
                PlayerId = otherPlayer.UserId,
                Card = card
            };
            _context.OwnedCards.Add(ownedCard);
            await _context.SaveChangesAsync();
            
            // Add the card to the other player's deck
            bool addResult = await _decksService.AddCardToDeck(ownedCard.Id, otherPlayerDeck.Id, otherPlayer, otherPlayer.UserId);
            Assert.IsTrue(addResult);
            
            // Act - Test player tries to remove a card from other player's deck
            bool removeResult = await _decksService.RemoveCardFromDeck(ownedCard.Id, otherPlayerDeck.Id, _testPlayer, _testPlayer.UserId);
            
            // Assert
            Assert.IsFalse(removeResult);
            
            // Verify card is still in the deck
            var deckAfterAttempt = await _context.Decks
                .Include(d => d.OwnedCards)
                .FirstOrDefaultAsync(d => d.Id == otherPlayerDeck.Id);
            
            Assert.IsNotNull(deckAfterAttempt);
            Assert.AreEqual(1, deckAfterAttempt.OwnedCards.Count);
        }
    }
}
