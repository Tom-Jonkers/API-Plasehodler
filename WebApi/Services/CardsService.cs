using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Services
{
	public class CardsService
    {
        private ApplicationDbContext _dbContext;

        public CardsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Card> GetPlayersCards(string userId)
        {
            // Stub: Pour l'intant, le stub retourne simplement les 8 premières cartes
            // L'implémentation réelle devra utiliser un service et retourner les cartes qu'un joueur possède
            // L'implémentation est la responsabilité de la personne en charge de la partie [Enregistrement et connexion]
            var temp = _dbContext.OwnedCards.Where(oc => oc.PlayerId == userId).Select(oc => oc.Card).ToList();
            return temp;
        }
        
        public IEnumerable<OwnedCard> GetPlayersOwnedCards(string userId)
        {
            return _dbContext.OwnedCards.Where(oc => oc.PlayerId == userId).ToList();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return _dbContext.Cards;
        }
    }
}

