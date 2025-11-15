using Microsoft.Extensions.Logging;
using Models.Models;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;

namespace Super_Cartes_Infinies.Combat
{
    public class EndMatchEvent : MatchEvent
    {
        public override string EventType { get { return "EndMatch"; } }
        public int WinningPlayerId { get; set; }
        public int MoneyReceivedByWinner { get; set; }
        public int MoneyReceivedByLoser { get; set; }
        public Deck? WinningPlayerCurrentDeck { get; set; }
        public Deck? LosingPlayerCurrentDeck { get; set; }
        public int ELOreceivedByWinner { get; set; }
        public int ELOreceivedbyLoser { get; set; }


        public EndMatchEvent(Match match, MatchPlayerData winningPlayerData, MatchPlayerData losingPlayerData, GameConfig gameConfig)
        {
            // Pour l'instant, on n'arrête pas la simulation sur le serveur lorsqu'on atteint la fin de la partie.
            // Pour éviter qu'un joueur qui a gagné, mais qui meurt dans le même tour ne donne la victoire à l'autre, on vérifie si le match est déjà terminé!
            if (match.IsMatchCompleted)
                return;

            WinningPlayerId = winningPlayerData.PlayerId;
            MoneyReceivedByWinner = gameConfig.MonnaieRecueVictoire;
            MoneyReceivedByLoser = gameConfig.MonnaieRecueDefaite;

            match.IsMatchCompleted = true;

            string userId;
            if (match.PlayerDataA.PlayerId == winningPlayerData.PlayerId)
                userId = match.UserAId;
            else
                userId = match.UserBId;

            match.WinnerUserId = userId;

            WinningPlayerCurrentDeck = winningPlayerData.Player.Decks.FirstOrDefault(d => d.IsCurrent)!;
            LosingPlayerCurrentDeck = losingPlayerData.Player.Decks.FirstOrDefault(d => d.IsCurrent)!;

            winningPlayerData.Player.Money += MoneyReceivedByWinner;

            winningPlayerData.Player.nbVictoires += 1;

            if (WinningPlayerCurrentDeck != null)
            {
                WinningPlayerCurrentDeck.nbVictoires += 1;
            }

            losingPlayerData.Player.Money += MoneyReceivedByLoser;

            losingPlayerData.Player.nbDefaites += 1;

            if (LosingPlayerCurrentDeck != null)
            {
                LosingPlayerCurrentDeck.nbDefaites += 1;
            }

            int winningPlayerELO = winningPlayerData.Player.ELO;
            int losingPlayerELO = losingPlayerData.Player.ELO;

            EloCalculator.CalculateELO(ref winningPlayerELO, ref losingPlayerELO, EloCalculator.GameOutcome.Win);

            ELOreceivedByWinner = winningPlayerELO - winningPlayerData.Player.ELO;
            ELOreceivedbyLoser = losingPlayerELO - losingPlayerData.Player.ELO;

            winningPlayerData.Player.ELO = winningPlayerELO;
            losingPlayerData.Player.ELO = losingPlayerELO;
        }

    }

    public class EloCalculator
    {
        public enum GameOutcome
        {
            Win = 1,
            Loss = 0
        }

        public static void CalculateELO(ref int p1Rating, ref int p2Rating, GameOutcome p1Outcome)
        {
            int eloK = 32;

            double expectation = ExpectationToWin(p1Rating, p2Rating);
            int delta = (int)(eloK * ((int)p1Outcome - expectation));

            p1Rating += delta;
            p2Rating -= delta;
        }

        private static double ExpectationToWin(int p1Rating, int p2Rating)
        {
            return 1 / (1 + Math.Pow(10, (p2Rating - p1Rating) / 400.0));
        }
    }
}
