using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PoisonEvent : MatchEvent
    {
        public override string EventType { get { return "Poison"; } }

        public PoisonEvent(PlayableCard attackingCard, PlayableCard defendingCard)
        {
            Events = new List<MatchEvent>();
            
            defendingCard.AddStatusValue(Status.POISON_ID, attackingCard.GetPowerValue(Power.POISON_ID));
        }
    }
}
