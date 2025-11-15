using Models.Interfaces;
using Models.Models;

namespace Super_Cartes_Infinies.Models
{
    public class PlayableCard : IModel
    {
        public PlayableCard()
        {

        }

        public PlayableCard(Card c)
        {
            Card = c;
            Health = c.Health;
            Attack = c.Attack;
        }

        public int Id { get; set; }
        public virtual Card Card { get; set; }
        public virtual List<PlayableCardStatus> PlayableCardStatus { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Index { get; set; }

        public bool HasPower(int powerId)
        {
            // Check if Card or CardPowers is null
            if (Card == null || Card.CardPowers == null)
                return false;

            // Check if the card possesses the specified power
            return Card.CardPowers.Exists(c => c.Power.Id == powerId);
        }

        public int GetPowerValue(int powerId)
        {
            // Retourne les valeur du pouvoir pour cette carte.
            // Simplement retourner 0 si la carte ne possède pas ce pouvoir.
            if (HasPower(powerId))
                return Card.CardPowers.Where(c => c.Power.Id == powerId).Single().Value;
            else
                return 0;
        }
        
        public bool HasStatus(int statusId)
        {
            if (Card == null || PlayableCardStatus == null)
                return false;

            return PlayableCardStatus.Exists(c => c.StatusId == statusId);
        }

        public int GetStatusValue(int statusId)
        {
            if (HasStatus(statusId))
                return PlayableCardStatus.Single(c => c.StatusId == statusId).Value;
            else
                return 0;
        }
        
        public void AddStatusValue(int statusId, int powerValue)
        {
            var playableCardStatus = PlayableCardStatus.SingleOrDefault(c => c.StatusId == statusId);
            
            if (HasStatus(statusId))
            {
                playableCardStatus.Value += powerValue;
                
                if (playableCardStatus.Value <= 0)
                {
                    PlayableCardStatus.Remove(playableCardStatus);
                }
            }
            else
            {
                PlayableCardStatus.Add(new PlayableCardStatus
                {
                    Value = powerValue,
                    StatusId = statusId
                });
            }
        }
    }
}

