using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Combat
{
    public class StartMatchEvent : MatchEvent
    {
        private static readonly Random Random = new Random();

        public override string EventType { get { return "StartMatch"; } }
        public StartMatchEvent(Match match, MatchPlayerData currentPlayerData, MatchPlayerData opposingPlayerData, int nbCardsToDraw, int nbManaPerTurn)
        {
            Events = new List<MatchEvent> { };

            Shuffle(currentPlayerData.CardsPile);
            Shuffle(opposingPlayerData.CardsPile);

            // TODO: Faire piger le nombre de cartes de la configuration (nbCardsToDraw) au DEUX joueurs
            for (int i = 0; i < nbCardsToDraw; i++)
            {
                Events.Add(new DrawCardEvent(currentPlayerData));
                Events.Add(new DrawCardEvent(opposingPlayerData));
            }

            Events.Add(new PlayerStartTurnEvent(currentPlayerData, nbManaPerTurn));
        }

        public static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}

