using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class CardHealEvent : MatchEvent
    {
        public override string EventType { get { return "CardHeal"; } }
        public int PlayerId { get; set; }
        public int PlayableCardId { get; set; }
        public int Health { get; set; }


        public CardHealEvent(PlayableCard healedCard, PlayableCard healingCard, MatchPlayerData currentPlayerData)
        {
            // Vie de la carte après le heal
            int postHealthCard = healedCard.Health + healingCard.GetPowerValue(Power.HEAL_ID);
            if (postHealthCard > healedCard.Card.Health)
            {
                healedCard.Health = healedCard.Card.Health;
            }
            else
            {
                healedCard.Health = postHealthCard;
            }

            PlayerId = currentPlayerData.PlayerId;
            PlayableCardId = healedCard.Id;
            Health = healedCard.Health;
        }
    }
}
