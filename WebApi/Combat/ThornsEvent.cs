using Models.Models;
using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Models;
namespace Super_Cartes_Infinies.Combat
{
    public class ThornsEvent : MatchEvent
    {
        public override string EventType { get { return "Thorns"; } }

        public ThornsEvent(PlayableCard attackingCard, PlayableCard defendingCard, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData)
        {
            this.Events = new List<MatchEvent>();

            this.Events.Add(new CardDamageEvent(
                defendingCard.GetPowerValue(Power.THORNS_ID),
                attackingCard,
                currentPlayerData,
                opposingPlayerData));
        }
    }
}
