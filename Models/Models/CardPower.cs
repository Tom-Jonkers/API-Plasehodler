using Super_Cartes_Infinies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Models
{
    public class CardPower
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        [JsonIgnore]
        public virtual Card Card { get; set; }
        public int PowerId { get; set; }
        public virtual Power Power { get; set; }
        public int Value { get; set; }
    }
}
