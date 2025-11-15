using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;

namespace Super_Cartes_Infinies.Services
{
    public class PaquetsService
    {
        private static Random _random = new Random();
        private ApplicationDbContext _dbContext;

        public PaquetsService(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public IEnumerable<Paquet> GetAllPaquets()
        {
            return _dbContext.Paquets.ToList();
        }


        public List<Card> OpenPack(Paquet paquet, IEnumerable<Card> allCards, string playerId)
        {
            var player = _dbContext.Players.FirstOrDefault(p => p.UserId == playerId);
            if (player == null)
            {
                throw new ArgumentException($"Aucun joueur trouvé avec l'ID {playerId}");
            }

            if (player.Money < paquet.Cost)
            {
                throw new InvalidOperationException("Le joueur n'a pas assez d'argent pour acheter ce paquet.");
            }

            player.Money -= paquet.Cost;

            // Définir les probabilités et quantités de base selon le type de paquet
            var probabilities = GetProbabilitiesForPack(paquet.Name);
            var rarities = GenerateRarities(paquet.NbCartes, (int)paquet.RareteParDefaut, probabilities, paquet.Name);

            // Obtenir des cartes aléatoires selon les raretés générées
            var cards = new List<Card>();
            foreach (var rarity in rarities)
            {
                var cardsOfRarity = allCards.Where(c => c.Rarete == rarity).ToList();
                if (cardsOfRarity.Any())
                {
                    cards.Add(cardsOfRarity[_random.Next(cardsOfRarity.Count)]);
                }
            }

            var ownedCards = cards.Select(card => new OwnedCard
            {
                CardId = card.Id,
                PlayerId = playerId.ToString()
            }).ToList();

            _dbContext.OwnedCards.AddRange(ownedCards);
            _dbContext.SaveChanges();

            return cards;
        }

        private List<Probability> GetProbabilitiesForPack(string packName)
        {
            var probabilities = _dbContext.Probabilities.Where(p => p.PaquetName == packName).ToList();

            if (!probabilities.Any())
            {
                throw new ArgumentException("Type de paquet inconnu.");
            }

            return probabilities;
        }

        private List<Raretes> GenerateRarities(int nbCards, int defaultRarity, List<Probability> probabilities, string packName)
        {
            var rarities = new List<Raretes>();

            // Ajouter les cartes obligatoires selon le type de paquet
            if (packName == "Normal")
            {
                rarities.Add(Raretes.Rare); // 1 carte rare obligatoire
            }
            else if (packName == "Super")
            {
                rarities.Add(Raretes.Épique); // 1 carte épique obligatoire
            }


            // Ajouter les quantités de base
            foreach (var probability in probabilities)
            {
                for (int i = 0; i < probability.BaseQty; i++)
                {
                    rarities.Add(probability.Rarity);
                }
            }

            // Remplir jusqu'à atteindre le nombre de cartes
            while (rarities.Count < nbCards)
            {
                var rarity = GetRandomRarity(probabilities);
                if (rarity == null)
                {
                    rarities.Add((Raretes)defaultRarity);
                }
                else
                {
                    rarities.Add(rarity.Value);
                }
            }

            return rarities;
        }

        private Raretes? GetRandomRarity(List<Probability> probabilities)
        {
            decimal x = (decimal)_random.NextDouble();

            foreach (var probability in probabilities)
            {
                if (x <= probability.Value)
                {
                    return probability.Rarity;
                }
                else
                {
                    x -= probability.Value;
                }

            }

            return null;
        }
    }
}
