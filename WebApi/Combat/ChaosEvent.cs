using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class ChaosEvent : MatchEvent
    {
        public override string EventType { get { return "Chaos"; } }
        public int PlayableCardId { get; set; }
        public int PlayerId { get; set; }
        public int newAttack { get; set; }
        public int newHealth { get; set; }

        public ChaosEvent(MatchPlayerData currentPlayerData, PlayableCard playableCard)
        {
            Events = new List<MatchEvent>();
            PlayableCardId = playableCard.Id;
            PlayerId = currentPlayerData.PlayerId;
            newAttack = playableCard.Health;
            newHealth = playableCard.Attack;
            
            playableCard.Health = newHealth;
            playableCard.Attack = newAttack;

            if (playableCard.Health <= 0)
            {
                Events.Add(new CardDeathEvent(playableCard, currentPlayerData, currentPlayerData));
            }
        }
    }
}
