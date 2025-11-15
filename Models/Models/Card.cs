using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.Interfaces;
using Models.Models;

namespace Super_Cartes_Infinies.Models
{
    public enum Raretes
    {
        Commune,
        Rare,
        Épique,
        Légendaire
    }

    public class Card : IModel
    {
        public Card() { }

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Attack { get; set; }
        public int Health { get; set; }
        public int Cost { get; set; }
        public string ImageUrl { get; set; } = "";

        public Raretes Rarete { get; set; }

        [ValidateNever]
        public virtual List<CardPower> CardPowers { get; set; }
    }
}

