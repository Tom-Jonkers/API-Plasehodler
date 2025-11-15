using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Models;
using Models.Models;
namespace Tests.Services
{
    [TestClass]
    public class PowerTests : BaseTests
	{
        public PowerTests()
        {
        }

        [TestInitialize]
        public void Init()
        {
            base.Init();
        }

        [TestMethod]
        public void FirstStrikeAttacks()
        {
            Power firstStrikePower = new Power
            {
                Id = Power.FIRST_STRIKE_ID
            };

            CardPower cardPower = new CardPower
            {
                Power = firstStrikePower,
                Card = _cardA
            };

            _cardA.CardPowers = new List<CardPower> { cardPower };

            // On réduit le Health de la carte B pour que la carte meurt
            _playableCardB.Health = _playableCardA.Attack;

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            // La carte A n'a pas été blessé car elle a attaqué et tué son advesaire avant qu'il n'est eu
            // le temps de réagir
            Assert.AreEqual(_cardA.Health, _playableCardA.Health);
            Assert.AreEqual(0, _playableCardB.Health);
        }

        [TestMethod]
        public void FirstStrikeAttackWithoutKill()
        {
            Power firstStrikePower = new Power
            {
                Id = Power.FIRST_STRIKE_ID
            };

            CardPower cardPower = new CardPower
            {
                Power = firstStrikePower,
                Card = _cardA
            };

            _cardA.CardPowers = new List<CardPower> { cardPower };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            // FirstStrike n'a aucun effet si l'attaquant ne tue pas le défenseur
            // Les deux cartes ont été blessées normalement
            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);
        }

        [TestMethod]
        public void FirstStrikeNeSertARienPourLaDefense()
        {
            Power firstStrikePower = new Power
            {
                Id = Power.FIRST_STRIKE_ID
            };

            CardPower cardPower = new CardPower
            {
                Power = firstStrikePower,
                Card = _cardB
            };

            _cardB.CardPowers = new List<CardPower> { cardPower };

            // On réduit le Health de la carte A pour que la carte meurt
            _playableCardA.Health = _playableCardB.Attack;

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            // FirstStrike n'a aucun effet si il est sur le défenseur
            Assert.AreEqual(0, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);
        }

        [TestMethod]
        public void ThornsAttackSimple()
        {
            Power thornsPower = new Power
            {
                Id = Power.THORNS_ID
            };

            // On donne le pouvoir Thorn au défenseur
            CardPower cardPower = new CardPower
            {
                Power = thornsPower,
                Card = _cardB,
                Value = 1
            };
            _cardB.CardPowers = new List<CardPower> { cardPower };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            Assert.AreEqual(_cardA.Health - _playableCardB.Attack - cardPower.Value, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);

            AssertBothCardsStillOnBattlefield();
        }

        [TestMethod]
        public void ThornsAttackAvecDesDegatsSuffisantPourTuerAttaquant()
        {
            Power thornsPower = new Power
            {
                Id = Power.THORNS_ID
            };

            // On donne le pouvoir Thorn au défenseur
            CardPower cardPower = new CardPower
            {
                Power = thornsPower,
                Card = _cardB,
                // On veut être certain que l'attaquant meurt par Thorns pendant le test
                Value = _cardA.Health
            };
            _cardB.CardPowers = new List<CardPower> { cardPower };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            Assert.AreEqual(1, _opposingPlayerData.Health);
            Assert.AreEqual(1, _currentPlayerData.Health);

            // Si l'attaquant meurt par les dégâts de Thorns, il n'a pas le temps de commencer à se battre et le défenseur ne reçoit aucun dégât
            Assert.AreEqual(_cardA.Health - cardPower.Value, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health, _playableCardB.Health);

            // 
            AssertCurrentPlayerCardDied();
        }

        [TestMethod]
        public void ThornsSertARienPourUneAttaque()
        {
            Power thornsPower = new Power
            {
                Id = Power.THORNS_ID
            };

            // On donne le pouvoir Thorn a l'attaquant
            CardPower cardPower = new CardPower
            {
                Power = thornsPower,
                Card = _cardA,
                Value = 3
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Les deux cartes ont simplement perdu les points de vues habituelles
            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);
        }

        [TestMethod]
        public void Heal()
        {
            Power healPower = new Power
            {
                Id = Power.HEAL_ID
            };

            // On donne le pouvoir Heal à l'attaquant
            CardPower cardPower = new CardPower
            {
                Power = healPower,
                Card = _cardB,
                Value = 3
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            var damagedPlayableCard = new PlayableCard(_cardB)
            {
                Id = 3
            };

            // On retire 2 PVs à l'attaquant et 4 PVs à l'autre carte de l'attaquant
            _playableCardA.Health -= 2;
            damagedPlayableCard.Health -= 4;

            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(damagedPlayableCard);

            _opposingPlayerData.BattleField.Add(_playableCardB);

            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            Assert.AreEqual(_currentPlayerData.PlayerId, playerTurnEvent.PlayerId);

            // _playableCardA devrait avoir retrouvé ses points de vie initiaux            
            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health);
            Assert.AreEqual(_cardB.Health - _playableCardA.Attack, _playableCardB.Health);

            // damagePlayableCard devrait avoir été guéri de 3 de ses 4 de dégâts
            Assert.AreEqual(_cardB.Health - 1, damagedPlayableCard.Health);

            // Le damagedPlayableCard tue le joueur adverse car il n'y avait pas de carte pour le protéger
            Assert.AreEqual(0, _opposingPlayerData.Health);
            Assert.AreEqual(1, _currentPlayerData.Health);

            // Toutes les cartes sont encore en jeu
            Assert.AreEqual(2, _currentPlayerData.BattleField.Count);
            Assert.AreEqual(0, _currentPlayerData.Graveyard.Count);
            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count);
            Assert.AreEqual(0, _opposingPlayerData.Graveyard.Count);
        }

        [TestMethod]
        public void NukePowerTriggersWhenCardIsKilled()
        {
            // Arrange: Create the Nuke power
            Power nukePower = new Power
            {
                Id = Power.NUKE_ID
            };

            // Assign the Nuke power to a card
            CardPower cardPower = new CardPower
            {
                Power = nukePower,
                Card = _cardB
            };
            _cardB.CardPowers = new List<CardPower> { cardPower };

            // Set the health of the card with the Nuke power
            _playableCardB.Health = 5;

            // Set the attack of the attacking card to match the health of the card with the Nuke power
            _playableCardA.Attack = 5;

            // Add the cards to the respective battlefields
            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            // Act: Simulate the end of the player's turn, triggering the attack and the Nuke power
            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Assert: All cards should be moved to the graveyard
            Assert.AreEqual(0, _currentPlayerData.BattleField.Count, "Current player's battlefield should be empty.");
            Assert.AreEqual(0, _opposingPlayerData.BattleField.Count, "Opposing player's battlefield should be empty.");
            Assert.AreEqual(3, _currentPlayerData.Graveyard.Count, "Current player's graveyard should contain their attacking card.");
            Assert.AreEqual(1, _opposingPlayerData.Graveyard.Count, "Opposing player's graveyard should contain the card with the Nuke power.");
        }

        [TestMethod]
        public void ChaosPowerTest()
        {
            // Arrange: Create the Chaos power
            Power chaosPower = new Power
            {
                Id = Power.CHAOS_ID
            };

            // Assign the Chaos power to a card
            CardPower cardPower = new CardPower
            {
                Power = chaosPower,
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            // Create another set of cards to verify chaos affects all cards
            Card cardC = new Card
            {
                Id = 3,
                Attack = 2,
                Health = 5,
                Name = "Carte C"
            };

            Card cardD = new Card
            {
                Id = 4,
                Attack = 6,
                Health = 1,
                Name = "Carte D"
            };

            PlayableCard playableCardC = new PlayableCard(cardC)
            {
                Id = 3
            };

            PlayableCard playableCardD = new PlayableCard(cardD)
            {
                Id = 4
            };

            // Store initial values for verification
            int initialAttackA = _playableCardA.Attack;
            int initialHealthA = _playableCardA.Health;
            int initialAttackB = _playableCardB.Attack;
            int initialHealthB = _playableCardB.Health;

            // Add all cards to the battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(playableCardC);
            _opposingPlayerData.BattleField.Add(_playableCardB);
            _opposingPlayerData.BattleField.Add(playableCardD);

            // Act: Trigger the Chaos power by ending the turn
            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Assert: All cards should have their attack and health values swapped
            Assert.AreEqual(initialHealthA, _playableCardA.Attack, "Card A's attack should be equal to its initial health");
            Assert.AreEqual(initialAttackA, _playableCardA.Health + _playableCardB.Attack, "Card A's health should be equal to its initial attack");
            
            Assert.AreEqual(initialHealthB, _playableCardB.Attack, "Card B's attack should be equal to its initial health");
            Assert.AreEqual(initialAttackB, _playableCardB.Health + _playableCardA.Attack, "Card B's health should be equal to its initial attack");
        }

        [TestMethod]
        public void ChaosPowerKillsLowHealthCardsTest()
        {
            // Arrange: Create the Chaos power`
            Power chaosPower = new Power
            {
                Id = Power.CHAOS_ID
            };

            // Assign the Chaos power to a card
            CardPower cardPower = new CardPower
            {
                Power = chaosPower,
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            // Create a card with 0 attack that will die when swapped
            Card cardC = new Card
            {
                Id = 3,
                Attack = 0,
                Health = 5,
                Name = "Carte C",
                CardPowers = new List<CardPower> { cardPower }
            };

            PlayableCard playableCardC = new PlayableCard(cardC)
            {
                Id = 3
            };

            // Add cards to the battlefield
            _currentPlayerData.BattleField.Add(playableCardC);

            // Act: Trigger the Chaos power by ending the turn
            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);

            // Assert: Card C should be in the graveyard since its health became 0
            Assert.AreEqual(1, _currentPlayerData.Graveyard.Count, "One card should be in the current player's graveyard");
            Assert.AreEqual(playableCardC.Id, _currentPlayerData.Graveyard[0].Id, "Card C should be in the graveyard");
            Assert.AreEqual(0, _currentPlayerData.BattleField.Count, "No card should remain on the current player's battlefield");
        }
        
        [TestMethod]
        public void EarthQuakePowerTest()
        {
            Power earthquakePower = new Power
            {
                Id = Power.EARTHQUAKE_ID
            };
        
            CardPower cardPower = new CardPower
            {
                Power = earthquakePower,
                Card = _cardA,
                Value = 2 
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
            
            var initPlayableCardBHealth = _playableCardB.Health;
        
            // Add all cards to the battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Trigger the EarthQuake power
            var eqEvent = new EarthQuakeEvent(_playableCardA, _currentPlayerData, _opposingPlayerData);

            Assert.AreEqual(initPlayableCardBHealth-2, _playableCardB.Health);

        }
        
        [TestMethod]
        public void EarthQuakePowerKillsOtherCardsTest()
        {
            // Arrange: Create the EarthQuake power
            Power earthquakePower = new Power
            {
                Id = Power.EARTHQUAKE_ID
            };
        
            // Assign the EarthQuake power to card A with a high damage value
            CardPower cardPower = new CardPower
            {
                Power = earthquakePower,
                Card = _cardA,
                Value = 5 // The earthquake will deal 5 damage to all other cards
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Create additional cards that will die from earthquake damage
            Card cardC = new Card
            {
                Id = 3,
                Attack = 2,
                Health = 3,
                Name = "Card C"
            };
        
            Card cardD = new Card
            {
                Id = 4,
                Attack = 3,
                Health = 6,
                Name = "Card D"
            };
        
            PlayableCard playableCardC = new PlayableCard(cardC)
            {
                Id = 3
            };
        
            PlayableCard playableCardD = new PlayableCard(cardD)
            {
                Id = 4
            };
        
            // Set card B's health so it will also die
            _playableCardB.Health = 3;
        
            // Add all cards to the battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _currentPlayerData.BattleField.Add(playableCardC);
            _opposingPlayerData.BattleField.Add(_playableCardB);
            _opposingPlayerData.BattleField.Add(playableCardD);
        
            // Act: Trigger the EarthQuake power by ending the turn
            var eqEvent = new EarthQuakeEvent(_playableCardA, _currentPlayerData, _opposingPlayerData);

            // Assert: Card A should be in the current player's graveyard
            Assert.IsTrue(_currentPlayerData.Graveyard.Exists(g => g.Id == _playableCardA.Id), "Card A should be in the graveyard");
        
            // Card C should be in the current player's graveyard too (died from earthquake)
            CollectionAssert.Contains(_currentPlayerData.Graveyard.Select(c => c.Id).ToList(), playableCardC.Id, "Card C should be in the graveyard");
        
            // Card B should be in the opposing player's graveyard
            Assert.IsTrue(_opposingPlayerData.Graveyard.Exists(g => g.Id == _playableCardB.Id), "Card B should be in the graveyard");
        
            // Card D should have survived but taken damage
            Assert.AreEqual(0, _currentPlayerData.BattleField.Count, "No cards should remain on the current player's battlefield");
            Assert.AreEqual(1, _opposingPlayerData.BattleField.Count, "Only card D should remain on the opposing player's battlefield");
            Assert.AreEqual(cardD.Health - cardPower.Value, playableCardD.Health, "Card D should have lost health from earthquake");
        }
        
        [TestMethod]
        public void RandomPainDamagesOneRandomOpponentCard()
        {
            // Arrange: Create the RandomPain power
            Power randomPainPower = new Power
            {
                Id = Power.RANDOM_PAIN_ID
            };

            // Assign the RandomPain power to card A
            CardPower cardPower = new CardPower
            {
                Power = randomPainPower,
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            // Create multiple opponent cards to test random targeting
            Card cardC = new Card
            {
                Id = 3,
                Attack = 2,
                Health = 5,
                Name = "Card C"
            };

            Card cardD = new Card
            {
                Id = 4,
                Attack = 3,
                Health = 4,
                Name = "Card D"
            };

            PlayableCard playableCardC = new PlayableCard(cardC)
            {
                Id = 3
            };

            PlayableCard playableCardD = new PlayableCard(cardD)
            {
                Id = 4
            };

            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
            _opposingPlayerData.BattleField.Add(playableCardC);
            _opposingPlayerData.BattleField.Add(playableCardD);

            // Store initial health values
            var initialHealthB = _playableCardB.Health;
            var initialHealthC = playableCardC.Health;
            var initialHealthD = playableCardD.Health;

            // Act: Trigger the RandomPain power
            var randomPainEvent = new RandomPainEvent(_playableCardA, _currentPlayerData, _opposingPlayerData);

            // Assert: Only one opponent card should be damaged and card A should die
            var totalDamagedCards = 0;
            if (_playableCardB.Health < initialHealthB) totalDamagedCards++;
            if (playableCardC.Health < initialHealthC) totalDamagedCards++;
            if (playableCardD.Health < initialHealthD) totalDamagedCards++;

            Assert.AreEqual(1, totalDamagedCards, "Exactly one opponent card should be damaged");
            Assert.IsTrue(_currentPlayerData.Graveyard.Contains(_playableCardA), "Card A should be in the graveyard");
        }
        
        
        [TestMethod]
        public void RandomPainKillsTargetCard()
        {
            // Arrange: Create and assign RandomPain power
            Power randomPainPower = new Power { Id = Power.RANDOM_PAIN_ID };
            CardPower cardPower = new CardPower
            {
                Power = randomPainPower,
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            // Set opponent card to low health
            _playableCardB.Health = 1;

            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);

            // Act: Trigger RandomPain
            var randomPainEvent = new RandomPainEvent(_playableCardA, _currentPlayerData, _opposingPlayerData);

            // Assert: Both cards should be in graveyards
            Assert.IsTrue(_currentPlayerData.Graveyard.Contains(_playableCardA), "Card A should be in the graveyard");
            Assert.IsTrue(_opposingPlayerData.Graveyard.Contains(_playableCardB), "Card B should be in the graveyard");
            Assert.AreEqual(0, _opposingPlayerData.BattleField.Count, "Opponent battlefield should be empty");
        }
        
        [TestMethod]
        public void CharmPowerMakesOpponentCardAttackOwner()
        {
            // Arrange: Create the Charm power
            Power charmPower = new Power
            {
                Id = Power.CHARM_ID
            };
        
            // Assign the Charm power to card A
            CardPower cardPower = new CardPower
            {
                Power = charmPower, 
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            _opposingPlayerData.Health = 20;
        
            // Set initial health values
            var initialOpponentHealth = _opposingPlayerData.Health;
            _playableCardB.Attack = 3;
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Trigger the Charm power
            var charmEvent = new CharmEvent(_playableCardA, _currentPlayerData, _opposingPlayerData, _match, new GameConfig());
        
            // Assert: Opponent card should damage its owner
            Assert.AreEqual(initialOpponentHealth - _playableCardB.Attack, _opposingPlayerData.Health);
            Assert.AreEqual(_currentPlayerData.Health, 1); // Current player health unchanged
        }
        
        [TestMethod]
        public void CharmPowerWithEmptyOpponentBattlefield()
        {
            // Arrange: Create and assign Charm power
            Power charmPower = new Power { Id = Power.CHARM_ID };
            CardPower cardPower = new CardPower
            {
                Power = charmPower,
                Card = _cardA
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            var initialOpponentHealth = _opposingPlayerData.Health;
        
            // Add only charming card to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
        
            // Act: Trigger Charm with empty opponent battlefield
            var charmEvent = new CharmEvent(_playableCardA, _currentPlayerData, _opposingPlayerData, _match, new GameConfig()); 
        
            // Assert: No damage should be dealt
            Assert.AreEqual(initialOpponentHealth, _opposingPlayerData.Health);
        }
        
        [TestMethod]
        public void CharmPowerSelectsRandomTargetFromMultipleCards()
        {
            // Arrange: Create and assign Charm power
            Power charmPower = new Power { Id = Power.CHARM_ID };
            CardPower cardPower = new CardPower
            {
                Power = charmPower,
                Card = _cardA  
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Create multiple opponent cards
            Card cardC = new Card
            {
                Id = 3,
                Attack = 2,
                Health = 5,
                Name = "Card C"
            };
        
            Card cardD = new Card
            {
                Id = 4,
                Attack = 3,
                Health = 4,
                Name = "Card D"
            };
        
            PlayableCard playableCardC = new PlayableCard(cardC) { Id = 3 };
            PlayableCard playableCardD = new PlayableCard(cardD) { Id = 4 };
        
            var initialOpponentHealth = _opposingPlayerData.Health;
            var totalExpectedDamage = _playableCardB.Attack + playableCardC.Attack + playableCardD.Attack;
        
            // Add all cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
            _opposingPlayerData.BattleField.Add(playableCardC);
            _opposingPlayerData.BattleField.Add(playableCardD);
        
            // Act: Trigger Charm
            var charmEvent = new CharmEvent(_playableCardA, _currentPlayerData, _opposingPlayerData, _match, new GameConfig());
        
            // Assert: Only one card's attack worth of damage should be dealt
            var actualDamage = initialOpponentHealth - _opposingPlayerData.Health;
            Assert.IsTrue(actualDamage > 0, "Some damage should be dealt");
            Assert.IsTrue(actualDamage < totalExpectedDamage, "Not all cards should attack");
            Assert.IsTrue(new[] { _playableCardB.Attack, playableCardC.Attack, playableCardD.Attack }.Contains(actualDamage));
        }
        
        [TestMethod]
        public void PoisonPowerAddsStatusToDefendingCard()
        {
            // Arrange: Create the Poison power
            Power poisonPower = new Power
            {
                Id = Power.POISON_ID
            };
        
            // Assign the Poison power to card A
            CardPower cardPower = new CardPower
            {
                Power = poisonPower,
                Card = _cardA,
                Value = 2
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Add both cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Trigger combat with poison card
            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
        
            // Assert: Defending card should have poison status
            Assert.AreEqual(2, _playableCardB.GetStatusValue(Status.POISON_ID));
        }
        
        [TestMethod]
        public void PoisonDamageAppliesEachTurn()
        {
            // Arrange: Create and assign Poison power
            Power poisonPower = new Power { Id = Power.POISON_ID };
            CardPower cardPower = new CardPower
            {
                Power = poisonPower,
                Card = _cardA,
                Value = 2
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };

            _playableCardB.Health = 8;
            var initialHealth = _playableCardB.Health;
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA); 
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Play two turns
            var turnEvent1 = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
            var turnEvent2 = new PlayerEndTurnEvent(_match, _opposingPlayerData, _currentPlayerData, NB_MANA_PER_TURN);
        
            // Assert: Card B should take poison damage each turn
            Assert.AreEqual(initialHealth - 6, _playableCardB.Health);
        }
        
        [TestMethod]
        public void PoisonStatusDecreasesEachTurn()
        {
            // Arrange: Create and assign Poison power
            Power poisonPower = new Power { Id = Power.POISON_ID };
            CardPower cardPower = new CardPower
            {
                Power = poisonPower,
                Card = _cardA,
                Value = 3
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act & Assert: Check poison decreases each turn
            var turnEvent1 = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
            Assert.AreEqual(3, _playableCardB.GetStatusValue(Status.POISON_ID));
        
            var turnEvent2 = new PlayerEndTurnEvent(_match, _opposingPlayerData, _currentPlayerData, NB_MANA_PER_TURN);
            Assert.AreEqual(2, _playableCardB.GetStatusValue(Status.POISON_ID));
        }
        
        [TestMethod]
        public void PoisonKillsCardWhenHealthReachesZero()
        {
            // Arrange: Create and assign Poison power
            Power poisonPower = new Power { Id = Power.POISON_ID };
            CardPower cardPower = new CardPower
            {
                Power = poisonPower,
                Card = _cardA,
                Value = 3
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Set card B health low enough to die from poison
            _playableCardB.Health = 2;
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Play turn
            var turnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
        
            // Assert: Card B should die and move to graveyard
            Assert.AreEqual(0, _opposingPlayerData.BattleField.Count);
            Assert.IsTrue(_opposingPlayerData.Graveyard.Contains(_playableCardB));
        }
        
        [TestMethod]
        public void StunStatusExpiredAfterOneTurn()
        {
            // Arrange: Create and assign Stun power
            Power stunPower = new Power { Id = Power.STUNNED_ID };
            CardPower cardPower = new CardPower
            {
                Power = stunPower,
                Card = _cardA,
                Value = 1
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Play two turns
            var turnEvent1 = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
            var turnEvent2 = new PlayerEndTurnEvent(_match, _opposingPlayerData, _currentPlayerData, NB_MANA_PER_TURN);
        
            // Assert: Stun should expire after the first turn
            Assert.AreEqual(0, _playableCardB.GetStatusValue(Status.STUNNED_ID));
            Assert.AreEqual(_cardA.Health - _playableCardB.Attack, _playableCardA.Health, "Card should attack normally after stun expires");
        }
        
        [TestMethod]
        public void StunCannotBeAppliedToAlreadyStunnedCard()
        {
            // Arrange: Create and assign Stun power
            Power stunPower = new Power { Id = Power.STUNNED_ID };
            CardPower cardPower = new CardPower
            {
                Power = stunPower,
                Card = _cardA,
                Value = 2
            };
            _cardA.CardPowers = new List<CardPower> { cardPower };
        
            // Pre-stun card B
            _playableCardB.AddStatusValue(Status.STUNNED_ID, 1);
        
            // Add cards to battlefield
            _currentPlayerData.BattleField.Add(_playableCardA);
            _opposingPlayerData.BattleField.Add(_playableCardB);
        
            // Act: Try to apply stun to already stunned card
            var playerTurnEvent = new PlayerEndTurnEvent(_match, _currentPlayerData, _opposingPlayerData, NB_MANA_PER_TURN);
        
            // Assert: Original stun duration should remain unchanged
            Assert.AreEqual(1, _playableCardB.GetStatusValue(Status.STUNNED_ID), "Stun duration should not stack");
        }
    }
}

