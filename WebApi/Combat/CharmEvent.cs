using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class CharmEvent : MatchEvent
    {
        public override string EventType { get { return "Charm"; } }

        public CharmEvent(PlayableCard charmingCard, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, Match match, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();
            
            // Select a random card from opponent's battlefield
            if (opposingPlayerData.BattleField.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(opposingPlayerData.BattleField.Count);
                PlayableCard charmTarget = opposingPlayerData.BattleField[randomIndex];
                
                // Make the charmed card attack its own player
                Events.Add(new PlayerDamageEvent(charmTarget.Attack, opposingPlayerData, opposingPlayerData, match, gameConfig));
            }
        }
    }
}
