using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class CardDeathEvent : MatchEvent
    {
        public override string EventType { get { return "CardDeath"; } }

        public int PlayableCardId { get; set; }
        public int PlayerId { get; set; }

        public CardDeathEvent(PlayableCard deadCard, MatchPlayerData grievingPlayer, MatchPlayerData winningPlayer)
        {
            this.Events = new List<MatchEvent>();
            PlayableCardId = deadCard.Id;
            PlayerId = grievingPlayer.PlayerId;

            grievingPlayer.Graveyard.Add(deadCard);
            grievingPlayer.RemoveCardFromBattleField(deadCard);
        }
    }
}
