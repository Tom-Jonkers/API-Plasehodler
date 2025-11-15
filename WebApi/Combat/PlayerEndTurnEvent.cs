using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PlayerEndTurnEvent : MatchEvent
    {
        public override string EventType { get { return "PlayerEndTurn"; } }
        public int PlayerId { get; set; }
        // L'évènement lorsqu'un joueur termine son tour
        public PlayerEndTurnEvent(Match match, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, int nbManaPerTurn, GameConfig gameConfig)
        {
            this.PlayerId = currentPlayerData.PlayerId;
            this.Events = new List<MatchEvent>();

            match.IsPlayerATurn = !match.IsPlayerATurn;

            this.Events.Add(new CombatEvent(match, currentPlayerData, opposingPlayerData, gameConfig));

            this.Events.Add(new PlayerStartTurnEvent(opposingPlayerData, nbManaPerTurn));
        }

        // Pour les tests
        public PlayerEndTurnEvent(Match match, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, int nbManaPerTurn)
        {
            this.PlayerId = currentPlayerData.PlayerId;
            this.Events = new List<MatchEvent>();

            match.IsPlayerATurn = !match.IsPlayerATurn;

            this.Events.Add(new CombatEvent(match, currentPlayerData, opposingPlayerData, new GameConfig()));

            this.Events.Add(new PlayerStartTurnEvent(opposingPlayerData, nbManaPerTurn));
        }

    }
}
