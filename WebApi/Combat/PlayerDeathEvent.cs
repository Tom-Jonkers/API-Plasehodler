using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PlayerDeathEvent : MatchEvent
    {
        public override string EventType { get { return "PlayerDeath"; } }

        public PlayerDeathEvent(MatchPlayerData currentPlayerData, MatchPlayerData damagedPlayerData, Match match, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();
            this.Events.Add(new EndMatchEvent(match, currentPlayerData, damagedPlayerData, gameConfig));
        }
    }
}
