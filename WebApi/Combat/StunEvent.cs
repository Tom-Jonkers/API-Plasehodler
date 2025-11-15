using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class StunEvent : MatchEvent
    {
        public override string EventType { get { return "Stun"; } }

        public StunEvent(PlayableCard attackingCard, PlayableCard defendingCard)
        {
            Events = new List<MatchEvent>();
            
            defendingCard.AddStatusValue(Status.STUNNED_ID, attackingCard.GetPowerValue(Power.STUNNED_ID));
        }
    }
}
