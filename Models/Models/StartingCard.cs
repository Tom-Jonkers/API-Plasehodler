using Models.Interfaces;
using System.Text.Json.Serialization;

namespace Super_Cartes_Infinies.Models
{
    public class StartingCard : IModel
    {
        public StartingCard() { }

        public int Id { get; set; }
        public required int CardId { get; set; }
        [JsonIgnore]
        public virtual Card Card { get; set; }
    }
}
