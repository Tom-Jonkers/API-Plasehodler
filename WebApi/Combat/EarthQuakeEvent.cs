using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class EarthQuakeEvent : MatchEvent
    {
        public override string EventType { get { return "EarthQuake"; } }

        public EarthQuakeEvent(PlayableCard playableCard, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData)
        {
            Events = new List<MatchEvent>();

            var currentPlayerCards = currentPlayerData.BattleField.Where(card => card.Id != playableCard.Id).ToList();
            for (int i = 0; i < currentPlayerCards.Count; i++)
            {
                var card = currentPlayerCards[i];
                Events.Add(new CardDamageEvent(playableCard.GetPowerValue(Power.EARTHQUAKE_ID), card, currentPlayerData, opposingPlayerData));
            }

            var opposingPlayerCards = opposingPlayerData.BattleField.ToList();
            for (int i = 0; i < opposingPlayerCards.Count; i++)
            {
                var card = opposingPlayerCards[i];
                Events.Add(new CardDamageEvent(playableCard.GetPowerValue(Power.EARTHQUAKE_ID), card, opposingPlayerData, currentPlayerData));
            }
            
            Events.Add(new CardDeathEvent(playableCard, currentPlayerData, opposingPlayerData));

        }
    }
}
