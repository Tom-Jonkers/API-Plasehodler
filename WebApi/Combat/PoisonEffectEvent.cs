using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PoisonEffectEvent : MatchEvent
    {
        public override string EventType
        {
            get { return "PoisonEffect"; }
        }

        public PoisonEffectEvent(PlayableCard card, MatchPlayerData defendingPlayerData)
        {
            Events = new List<MatchEvent>();

            Events.Add(new CardDamageEvent(card.GetStatusValue(Status.POISON_ID), card, defendingPlayerData, defendingPlayerData));

            card.AddStatusValue(Status.POISON_ID, -1);
        }
    }
}