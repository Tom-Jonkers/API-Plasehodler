namespace WebApi.Models
{
    public class PairOfPlayers
    {
        public PlayerInfo Player1 { get; set; }
        public PlayerInfo Player2 { get; set; }

        public PairOfPlayers(PlayerInfo player1, PlayerInfo player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
    }
}
