using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.Interfaces;
using Models.Models;

namespace Super_Cartes_Infinies.Models
{
	public class Player : IModel
    {
		public Player()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; } = "";
		public required string UserId { get; set; }
		[JsonIgnore]
		public virtual IdentityUser User { get; set; }

		public int Money { get; set; }

		public int ELO { get; set; }

        [ValidateNever]
		public virtual List<Deck> Decks { get; set; }

		public int nbVictoires { get; set; }

        public int nbDefaites { get; set; }

    }
}

