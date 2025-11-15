using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Super_Cartes_Infinies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Deck
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsCurrent { get; set; }

        public int PlayerId { get; set; }

        [ValidateNever] public virtual List<OwnedCard> OwnedCards { get; set; } = new List<OwnedCard>();

        [JsonIgnore]
        public virtual Player Player { get; set; }

        public int nbVictoires { get; set; }

        public int nbDefaites { get; set; }
    }
}
