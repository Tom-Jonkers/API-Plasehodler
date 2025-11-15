using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PlayCardEvent : MatchEvent
    {
        public override string EventType { get { return "PlayCard"; } }
        public int PlayableCardId { get; set; }
        public int PlayerId { get; set; }
        public int PlayerMana { get; set; }

        public PlayCardEvent(MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, int playableCardId)
        {
            Events = new List<MatchEvent>();

            var playableCard = currentPlayerData.Hand.Where(c => c.Id == playableCardId).Single();
             
            PlayableCardId = playableCardId;
            PlayerId = currentPlayerData.PlayerId;

            currentPlayerData.Hand.Remove(playableCard);
            currentPlayerData.Mana -= playableCard.Card.Cost;
            currentPlayerData.AddCardToBattleField(playableCard);

            PlayerMana = currentPlayerData.Mana;

            if (playableCard.HasPower(Power.EARTHQUAKE_ID))
            {
                Events.Add(new EarthQuakeEvent(playableCard, currentPlayerData, opposingPlayerData));
            }
            
            if (playableCard.HasPower(Power.RANDOM_PAIN_ID))
            {
                Events.Add(new RandomPainEvent(playableCard, currentPlayerData, opposingPlayerData));
            }
        }
    }
}
