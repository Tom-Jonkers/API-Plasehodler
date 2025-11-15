using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class CardDamageEvent : MatchEvent
    {
        public override string EventType { get { return "CardDamage"; } }

        public int CardHealth { get; set; }
        public int PlayableCardId { get; set; }
        public int PlayerId { get; set; }

        public CardDamageEvent(int damage, PlayableCard damagedCard, MatchPlayerData damagedPlayerData, MatchPlayerData nonDamagedPlayerData)
        {
            this.Events = new List<MatchEvent>();
            PlayableCardId = damagedCard.Id;
            PlayerId = damagedPlayerData.PlayerId;

            damagedCard.Health -= damage;
            CardHealth = damagedCard.Health;
            if (damagedCard.Health <= 0)
            {
                // Si la carte morte possède Nuke
                if (damagedCard.HasPower(Power.NUKE_ID))
                {
                    this.Events.Add(new NukeEvent(damagedPlayerData, nonDamagedPlayerData));
                }
                else
                {
                    this.Events.Add(new CardDeathEvent(damagedCard, damagedPlayerData, nonDamagedPlayerData));
                }
                
            }
        }
    }
}
