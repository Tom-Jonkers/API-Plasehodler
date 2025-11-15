using Microsoft.EntityFrameworkCore;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Services
{
	public class StartingCardsService
    {
        private ApplicationDbContext _dbContext;

        public StartingCardsService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public List<Card> GetStartingCards()
        {
            // Stub: Pour l'intant, le stub retourne simplement les 7 premières cartes
            // L'implémentation réelle devra retourner les cartes référées par les starting cards configuré par l'administarteur
            // L'implémentation est la responsabilité de la personne en charge de la partie [Administration MVC]
            var startingCards = _dbContext.StartingCards
                                               .Include(sc => sc.Card)
                                               .ToList();

            // Crée une liste pour stocker les cartes référencées
            var startingCardList = new List<Card>();

            // Boucle sur chaque StartingCard et ajoute la Card associée à la liste
            foreach (var startingCard in startingCards)
            {
                if (startingCard.Card != null)
                {
                    startingCardList.Add(startingCard.Card);
                }
            }

            // Retourne la liste des cartes référencées
            return startingCardList;
        }
    }
}

