using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace Super_Cartes_Infinies.Combat
{
    public class CardActivationEvent : MatchEvent
    {
        public override string EventType { get { return "CardActivation"; } }
        public int PlayableCardId { get; set; }
        public int PlayerId { get; set; }

        public CardActivationEvent(int cardIndex, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, Match match, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();
            var orderedBattlefield = currentPlayerData.GetOrderedBattleField().ElementAtOrDefault(cardIndex);

            if (orderedBattlefield.HasStatus(Status.STUNNED_ID))
            {
                orderedBattlefield.AddStatusValue(Status.STUNNED_ID, -1);
                
                if (orderedBattlefield.HasStatus(Status.POISON_ID))
                {
                    Events.Add(new PoisonEffectEvent(orderedBattlefield, currentPlayerData));
                }
                
                return;
            }
            
            if (orderedBattlefield != null)
            {
                PlayableCardId = orderedBattlefield.Id;
                PlayerId = currentPlayerData.PlayerId;
                // Si la carte a Heal
                if (currentPlayerData.GetOrderedBattleField().ElementAt(cardIndex).HasPower(Power.HEAL_ID))
                {
                    this.Events.Add(new HealEvent(currentPlayerData.GetOrderedBattleField().ElementAt(cardIndex), currentPlayerData));
                }
                
                this.Events.Add(new AttackEvent(cardIndex, currentPlayerData, opposingPlayerData, match, gameConfig));

                var card = currentPlayerData.GetOrderedBattleField().ElementAtOrDefault(cardIndex);
                    
                if (card != null && card.HasStatus(Status.POISON_ID))
                {
                    Events.Add(new PoisonEffectEvent(currentPlayerData.GetOrderedBattleField().ElementAt(cardIndex), currentPlayerData));
                }
            }

        }
    }
}
