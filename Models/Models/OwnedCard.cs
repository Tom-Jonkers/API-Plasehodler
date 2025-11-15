using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.Interfaces;
using Super_Cartes_Infinies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Models
{
    public class OwnedCard : IModel
    {
        public OwnedCard() { }

        public int Id { get; set; }
        public required int CardId { get; set; }
        public string PlayerId { get; set; }

        public virtual Card Card { get; set; }

        [JsonIgnore]
        public virtual List<Deck> Decks { get; set; }
    }
}
