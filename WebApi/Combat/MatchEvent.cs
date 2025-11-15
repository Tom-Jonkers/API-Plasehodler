using System.Text.Json.Serialization;

namespace Super_Cartes_Infinies.Combat
{
    [JsonDerivedType(typeof(AttackEvent))]
    [JsonDerivedType(typeof(CardActivationEvent))]
    [JsonDerivedType(typeof(CardDamageEvent))]
    [JsonDerivedType(typeof(CardDeathEvent))]
    [JsonDerivedType(typeof(CardHealEvent))]
    [JsonDerivedType(typeof(CombatEvent))]
    [JsonDerivedType(typeof(DrawCardEvent))]
    [JsonDerivedType(typeof(EndMatchEvent))]
    [JsonDerivedType(typeof(FirstStrikeEvent))]
    [JsonDerivedType(typeof(GainManaEvent))]
    [JsonDerivedType(typeof(HealEvent))]
    [JsonDerivedType(typeof(NukeEvent))]
    [JsonDerivedType(typeof(PlayCardEvent))]
    [JsonDerivedType(typeof(PlayerDamageEvent))]
    [JsonDerivedType(typeof(PlayerDeathEvent))]
    [JsonDerivedType(typeof(PlayerEndTurnEvent))]
    [JsonDerivedType(typeof(PlayerStartTurnEvent))]
    [JsonDerivedType(typeof(StartMatchEvent))]
    [JsonDerivedType(typeof(SurrenderEvent))]
    [JsonDerivedType(typeof(ThornsEvent))]
    [JsonDerivedType(typeof(ChaosEvent))]
    [JsonDerivedType(typeof(EarthQuakeEvent))]
    [JsonDerivedType(typeof(RandomPainEvent))]
    [JsonDerivedType(typeof(CharmEvent))]
    [JsonDerivedType(typeof(PoisonEvent))]
    [JsonDerivedType(typeof(PoisonEffectEvent))]
    [JsonDerivedType(typeof(StunEvent))]

    public abstract class MatchEvent
    {
        public abstract string EventType { get; }

        public MatchEvent()
        {
        }

        public List<MatchEvent>? Events { get; set; }
    }
}
