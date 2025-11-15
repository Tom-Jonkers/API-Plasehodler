using Microsoft.AspNetCore.Identity;
using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Tests.Services
{
    public class BaseTests
	{
        protected const int STARTING_PLAYER_HEALTH = 1;
        protected const int NB_MANA_PER_TURN = 3;

        protected MatchPlayerData _currentPlayerData, _opposingPlayerData;
        protected Match _match;
        protected Card _cardA, _cardB;
        protected PlayableCard _playableCardA, _playableCardB;

        public BaseTests()
        {
        }

        protected void Init()
        {
            Player currentPlayer = new Player()
            {
                UserId = "1",
                Decks = new List<Models.Models.Deck>()
            };
            var deck = new Models.Models.Deck
            {
                Name = "base deck",
                Player = currentPlayer,
                nbVictoires = 0,
                nbDefaites = 0
            };
            currentPlayer.Decks.Add(deck);
            _currentPlayerData = new MatchPlayerData(1)
            {
                Health = STARTING_PLAYER_HEALTH,
                Player = currentPlayer,
                Mana = 2
            };

            Player opposingPlayer = new Player()
            {
                UserId = "2",
                Decks = new List<Models.Models.Deck>()
            };
            opposingPlayer.Decks.Add(deck);
            _opposingPlayerData = new MatchPlayerData(2)
            {
                Health = STARTING_PLAYER_HEALTH,
                Player = opposingPlayer,
                Mana = 0
            };

            // Le match n'est pas utilisé par ce test, on peut simplement en créer un sans initializer les données
            _match = new Match
            {
                UserAId = "UserAId",
                UserBId = "UserBId",
                PlayerDataA = _currentPlayerData,
                PlayerDataB = _opposingPlayerData
            };

            _cardA = new Card
            {
                Id = 42,
                Attack = 2,
                Health = 3,
                Cost = 1
            };

            _cardB = new Card
            {
                Id = 43,
                Attack = 1,
                Health = 5,
                Cost = 1
            };

            _playableCardA = new PlayableCard(_cardA)
            {
                Id = 1,
                PlayableCardStatus = new List<PlayableCardStatus>()
            };
            _playableCardB = new PlayableCard(_cardB)
            {
                Id = 2,
                PlayableCardStatus = new List<PlayableCardStatus>()
            };
        }

        protected void AssertBothCardsStillOnBattlefield()
        {
            Assert.AreEqual(1, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(0, _currentPlayerData.Graveyard.Count);

            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(0, _opposingPlayerData.Graveyard.Count);
        }

        protected void AssertCurrentPlayerCardDied()
        {
            Assert.AreEqual(0, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(1, _currentPlayerData.Graveyard.Count);

            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(0, _opposingPlayerData.Graveyard.Count);
        }

        protected void AssertOpposingPlayerCardDied()
        {
            Assert.AreEqual(1, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(0, _currentPlayerData.Graveyard.Count);

            Assert.AreEqual(0, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(1, _opposingPlayerData.Graveyard.Count);
        }

        protected void AssertBothPlayersStillHaveFullHealth()
        {
            Assert.AreEqual(STARTING_PLAYER_HEALTH, _opposingPlayerData.Health);
            Assert.AreEqual(STARTING_PLAYER_HEALTH, _currentPlayerData.Health);
        }
    }
}

