using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class NukeEvent : MatchEvent
    {
        public override string EventType { get { return "Nuke"; } }

        public NukeEvent(MatchPlayerData opposingPlayer, MatchPlayerData currentPlayer)
        {
            this.Events = new List<MatchEvent>();
            // Tuer toute les cartes de currentPlayer
            for (int i = currentPlayer.GetOrderedBattleField().Count() - 1; i >= 0; i--)
            {
                this.Events.Add(new CardDeathEvent(currentPlayer.GetOrderedBattleField().ElementAt(i), currentPlayer, opposingPlayer));
            }

            // Tuer toute les cartes de opposingPlayer
            for (int i = opposingPlayer.GetOrderedBattleField().Count() - 1; i >= 0; i--)
            {
                this.Events.Add(new CardDeathEvent(opposingPlayer.GetOrderedBattleField().ElementAt(i), opposingPlayer, currentPlayer));
            }
        }
    }
}
