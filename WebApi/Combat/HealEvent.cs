using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class HealEvent : MatchEvent
    {
        public override string EventType { get { return "Heal"; } }

        public HealEvent(PlayableCard card, MatchPlayerData currentPlayerData)
        {
            this.Events = new List<MatchEvent>();
            foreach (PlayableCard healedCard in currentPlayerData.GetOrderedBattleField())
            {
                this.Events.Add(new CardHealEvent(healedCard, card, currentPlayerData));
            }
        }
    }
}
