namespace WebApi.Models
{
    public class PlayerInfo
    {
        public int ELO { get; set; }
        public double attente { get; set; }
        public string? UserID { get; set; }
        public string? ConnexionID { get; set; }

        public PlayerInfo(string userId, string connexionId, int elo)
        {
            UserID = userId;
            ConnexionID = connexionId;
            ELO = elo;
            attente = 0;
        }
    }
}
