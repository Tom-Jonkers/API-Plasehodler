using Models.Models;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class AttackEvent : MatchEvent
    {
        public override string EventType { get { return "Attack"; } }

        public AttackEvent(int cardIndex, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, Match match, GameConfig gameConfig)
        {
            this.Events = new List<MatchEvent>();

            PlayableCard attackingCard = currentPlayerData.GetOrderedBattleField().ElementAt(cardIndex);

            if (attackingCard.HasPower(Power.CHAOS_ID))
            {
                for (int i = currentPlayerData.BattleField.Count-1; i >= 0; i--)
                {
                    Events.Add(new ChaosEvent(currentPlayerData, currentPlayerData.BattleField[i]));
                }
                
                for (int i = opposingPlayerData.BattleField.Count-1; i >= 0; i--)
                {
                    Events.Add(new ChaosEvent(opposingPlayerData, opposingPlayerData.BattleField[i]));
                }
            }

            if (attackingCard.HasPower(Power.CHARM_ID))
            {
                Events.Add(new CharmEvent(attackingCard, currentPlayerData, opposingPlayerData, match, gameConfig));
            }

            // La carte a-t-elle une autre carte devant elle?
            if (opposingPlayerData.GetOrderedBattleField().Count() - 1 < cardIndex)
            {
                // Pas de carte devant, on attaque le joueur
                Events.Add(new PlayerDamageEvent(attackingCard.Attack, currentPlayerData, opposingPlayerData, match, gameConfig));
            }
            else
            {
                // Une carte en avant, on attaque la carte et elle se défend

                PlayableCard defendingCard = opposingPlayerData.GetOrderedBattleField().ElementAt(cardIndex);

                if (attackingCard.HasPower(Power.POISON_ID))
                {
                    Events.Add(new PoisonEvent(attackingCard, defendingCard));
                }
                
                if (attackingCard.HasPower(Power.STUNNED_ID) && !defendingCard.HasStatus(Status.STUNNED_ID))
                {
                    Events.Add(new StunEvent(attackingCard, defendingCard));
                }

                // La carte adverse a-t-elle Thorns?
                if (defendingCard.HasPower(Power.THORNS_ID))
                {
                    // Oui, la carte adverse se défend
                    Events.Add(new ThornsEvent(attackingCard, defendingCard, currentPlayerData, opposingPlayerData));
                }

                // L'attaque ne continue que si la carte est encore en vie
                if (!currentPlayerData.Graveyard.Contains(attackingCard))
                {
                    Events.Add(new CardDamageEvent(attackingCard.Attack, defendingCard, opposingPlayerData, currentPlayerData));

                    // La carte attaquante a-t-elle First Strike?
                    if (attackingCard.HasPower(Power.FIRST_STRIKE_ID))
                    {
                        // First Strike, l'autre carte n'attaque que si elle est encore en vie
                        if (!opposingPlayerData.Graveyard.Contains(defendingCard))
                            Events.Add(new CardDamageEvent(defendingCard.Attack, attackingCard, currentPlayerData, opposingPlayerData));
                    }
                    else
                    {
                        // Pas de First Strike
                        Events.Add(new CardDamageEvent(defendingCard.Attack, attackingCard, currentPlayerData, opposingPlayerData));
                    }
                }
            }
        }   
    }
}
