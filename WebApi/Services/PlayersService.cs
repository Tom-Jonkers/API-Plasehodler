using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Services
{
	public class PlayersService
    {
        private ApplicationDbContext _dbContext;
        private StartingCardsService _startingCardsService;


        public PlayersService(ApplicationDbContext context, StartingCardsService startingCardsService)
        {
            _dbContext = context;
            _startingCardsService = startingCardsService;
        }

        public Player CreatePlayer(IdentityUser user, GameConfig gameConfig)
        {
            Player p = new Player()
            {
                Id = 0,
                UserId = user.Id,
                Name = user.Email!,
                Money = gameConfig.MonnaieRecueCreation,
                nbDefaites = 0,
                nbVictoires = 0,
                ELO = 1000
            };

            // TODO: Utilisez le service StartingCardsService pour obtenir les cartes de départ
            // TODO: Ajoutez ces cartes au joueur en utilisant le modèle OwnedCard que vous allez devoir ajouter

            var deck = new Deck()
            {
                Name = "Départ",
                IsCurrent = true,
                Player = p
            };

            var startingCardList = _startingCardsService.GetStartingCards();

            foreach (var card in startingCardList)
            {
                OwnedCard ownedCard = new OwnedCard { CardId = card.Id, PlayerId = p.UserId, Card = card };
                deck.OwnedCards.Add(ownedCard);
                _dbContext.Add(ownedCard);
            }

            _dbContext.Add(deck);
            _dbContext.Add(p);
            _dbContext.SaveChanges();

            return p;
        }

        public virtual Player GetPlayerFromUserId(string userId)
        {
            return _dbContext.Players.SingleOrDefault(p => p.UserId == userId);
        }

        public Player GetPlayerFromUserName(string userName)
        {
            return _dbContext.Players.Single(p => p.User!.UserName == userName);
        }
    }
}

