using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Models;

namespace Tests.Services
{
    [TestClass]
    public class CombatTests : BaseTests
	{
        public CombatTests()
        {
        }

        [TestInitialize]
        public void Init()
        {
            base.Init();
        }

        [TestMethod]
        public void PlayerIsGainingManaAtTheBeginningOfHisTurn()
        {
            // L'adversaire n'a pas encore de Mana
            Assert.AreEqual(0, _opposingPlayerData.Mana);

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // L'adversaire a maintenant du Mana
            Assert.AreEqual(NB_MANA_PER_TURN, _opposingPlayerData.Mana);
        }


        [TestMethod]
        public void TurnWithBasicFightTest()
        {
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            // L'adversaire n'a pas encore de Mana
            Assert.AreEqual(0, _opposingPlayerData.Mana);

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Les joueurs ont encore leur health
            AssertBothPlayersStillHaveFullHealth();

            // L'adversaire a maintenant du Mana
            Assert.AreEqual(NB_MANA_PER_TURN, _opposingPlayerData.Mana);

            // Tester la perte de point de vie sur les deux cartes
            int cardAHealth = _cardA.Health - _playableCardB.Attack;
            int cardBHealth = _cardB.Health - _playableCardA.Attack;
            Assert.AreEqual(cardAHealth, _playableCardA.Health);
            Assert.AreEqual(cardBHealth, _playableCardB.Health);

            // Les deux cartes devraient encore être en jeu (en vie)
            AssertBothCardsStillOnBattlefield();
        }

        [TestMethod]
        public void TurnWithTwoAgainstOneTest()
        {
            int opposingPlayerHealth = 10;
            _opposingPlayerData.Health = opposingPlayerHealth;

            var playableCardA2 = new PlayableCard(_cardA)
            {
                Id = 3
            };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(playableCardA2);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Le joueur donc c'est le tour a encore son health
            Assert.AreEqual(STARTING_PLAYER_HEALTH, _currentPlayerData.Health);
            // Le joueur adverse a perdu du health, il s'est fait attaquer par la deuxième carte du joueur
            Assert.AreEqual(opposingPlayerHealth - playableCardA2.Attack, _opposingPlayerData.Health);

            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);
            // La 2e carte du joueur courant a directement attaqué le joueur adverse et n'a pas reçu de dégât
            Assert.AreEqual(_cardA.Health, playableCardA2.Health);

            // 2 cards on the battle field for the current player
            Assert.AreEqual(2, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(0, _currentPlayerData.Graveyard.Count);
            
            // 1 card on the battle field for the opposing player
            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(0, _opposingPlayerData.Graveyard.Count);
        }

        [TestMethod]
        public void TurnWithCardDeathTest()
        {
            // On réduit l'attaque de B pour éviter que la carte A meurt
            _cardB.Attack = 1;
            // On réduit le nombre de health pour que la carte B meurt (exactement l'attaque de la carte A)
            _playableCardB.Health = _playableCardA.Attack;

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Les joueurs ont encore leurs health
            AssertBothPlayersStillHaveFullHealth();

            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health);
            Assert.AreEqual(0, _playableCardB.Health);

            AssertOpposingPlayerCardDied();
        }

        [TestMethod]
        // Le joueur A joue une carte dans ses mains alors que son adversaire a seulement 1 de health
        // Lorsque le tour s'effectue, le joueur B perd son dernier point de health et la victoire va au joueur A
        public void KillPlayerTest()
        {
            // On donne assez d'attaque à la carte A pour pouvoir tuer l'adversaire
            _playableCardA.Attack = _opposingPlayerData.Health;

            _currentPlayerData.BattleField.Add(_playableCardA);
            // On n'ajoute PAS la carte B sur le BattleField

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // L'adversaire n'a plus de health
            Assert.AreEqual(0, _opposingPlayerData.Health);
            Assert.AreEqual(STARTING_PLAYER_HEALTH, _currentPlayerData.Health);
            
            // Le match est terminé car un joueur n'a plus de health
            Assert.AreEqual(true, _match.IsMatchCompleted);
            // C'est le userA qui a gagné
            Assert.AreEqual(_match.UserAId, _match.WinnerUserId);
        }

        [TestMethod]
        public void TurnWithTwoAgainstOneWithFirstCardDyingTest()
        {
            int opposingPlayerHealth = 10;
            _opposingPlayerData.Health = opposingPlayerHealth;

            // On s'assure que la carte A va être détruite
            _playableCardA.Health = _playableCardB.Attack;

            var playableCardA2 = new PlayableCard(_cardA)
            {
                Id = 3
            };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(playableCardA2);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Le joueur dont c'est le tour a encore son health
            Assert.AreEqual(STARTING_PLAYER_HEALTH, _currentPlayerData.Health);
            // Le joueur adverse a perdu du health, il s'est fait attaquer par la deuxième carte du joueur
            // Même si la première carte de l'attaquant est morte (Les cartes ne bougent pas pendant le combat!)
            Assert.AreEqual(opposingPlayerHealth - playableCardA2.Attack, _opposingPlayerData.Health);

            Assert.AreEqual(0, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);
            // La 2e carte du joueur courant a directement attaqué le joueur adverse et n'a pas reçu de dégât
            Assert.AreEqual(_cardA.Health, playableCardA2.Health);

            // 1 carte sur le BattleField et un autre dans le Graveyard pour le joueur courant
            Assert.AreEqual(1, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(1, _currentPlayerData.Graveyard.Count);

            // 1 carte sur le BattleField pour l'adversaire
            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(0, _opposingPlayerData.Graveyard.Count);
        }
    }
}

