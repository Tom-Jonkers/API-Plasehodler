using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class PlayerDamageEvent : MatchEvent
    {
        public override string EventType { get { return "PlayerDamage"; } }

        public int PlayerHealth { get; set; }
        public int PlayerId { get; set; }

        public PlayerDamageEvent(int damage, MatchPlayerData currentPlayerData, MatchPlayerData damagedPlayerData, Match match, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();

            damagedPlayerData.Health -= damage;

            if (damagedPlayerData.Health <= 0)
            {
                damagedPlayerData.Health = 0;
                this.Events.Add(new PlayerDeathEvent(currentPlayerData, damagedPlayerData, match, gameConfig));
            }
            PlayerHealth = damagedPlayerData.Health;
            PlayerId = damagedPlayerData.PlayerId;
        }
    }
}
