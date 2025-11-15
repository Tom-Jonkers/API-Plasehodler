using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class CombatEvent : MatchEvent
    {
        public override string EventType { get { return "Combat"; } }

        public CombatEvent(Match match, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();

            // Activer toute les cartes
            for (int i = currentPlayerData.GetOrderedBattleField().Count() - 1; i >= 0; i--)
            {
                this.Events.Add(new CardActivationEvent(i, currentPlayerData, opposingPlayerData, match, gameConfig));
            }

           
        }
    }
}
