using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class RandomPainEvent : MatchEvent
    {
        public override string EventType { get { return "RandomPain"; } }

        public RandomPainEvent(PlayableCard playableCard, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData)
        {
            Events = new List<MatchEvent>();
            
            if (opposingPlayerData.BattleField.Count > 0)
            {
                var opposingCards = opposingPlayerData.BattleField.ToList();
                
                var random = new Random();
                int randomIndex = random.Next(opposingCards.Count);
                var targetCard = opposingCards[randomIndex];
                
                int damage = random.Next(1, 7);
                
                Events.Add(new CardDamageEvent(
                    damage, 
                    targetCard, 
                    opposingPlayerData, 
                    currentPlayerData));
            }

            Events.Add(new CardDeathEvent(playableCard, currentPlayerData, opposingPlayerData));

        }
    }
}
