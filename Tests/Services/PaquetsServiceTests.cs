using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Models;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Super_Cartes_Infinies.Services.Tests
{
    [TestClass()]
    public class PaquetsServiceTests
    {
        private ApplicationDbContext _db;
        private PaquetsService _paquetsService;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Server=(localdb)\\MSSQLLocalDB;Database=SuperCartesInfinies;Trusted_Connection=True;MultipleActiveResultSets=True")
                .Options;

            _db = new ApplicationDbContext(options);

            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            _paquetsService = new PaquetsService(_db);
        }


        [TestCleanup]
        public void Dispose()
        {
            _db.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OpenPack_ShouldThrowException_WhenPlayerHasNotEnoughMoney()
        {
            // Arrange
            var paquet = _db.Paquets.First();
            var playerId = "User1Id";
            var cards = _db.Cards.ToList();
            // Act
            _paquetsService.OpenPack(paquet, cards, playerId);

            // Assert
            // L'exception InvalidOperationException est attendue, donc aucun Assert supplémentaire n'est nécessaire
        }

        [TestMethod()]
        public void OpenPack_SuperPack_ShouldContainNoCommonCards_AndAtLeastOneEpicCard()
        {
            var paquet = _db.Paquets.First(p => p.Name == "Super");
            var playerId = "User2Id";
            var allCards = _db.Cards.ToList();

            // Act
            var result = _paquetsService.OpenPack(paquet, allCards, playerId);

            // Assert
            Assert.IsTrue(result.All(c => c.Rarete != Raretes.Commune), "Toutes les cartes doivent être d'une rareté supérieure à Commune.");
            Assert.IsTrue(result.Any(c => c.Rarete == Raretes.Épique), "Au moins une carte doit être Épique.");
        }

        [TestMethod()]
        public void OpenPack_ShouldGiveCorrectNumberOfCards_AndDeductCorrectAmountOfMoney()
        {
            // Arrange
            var allPaquets = _db.Paquets.ToList();
            var playerId = "User2Id";
            var player = _db.Players.First(p => p.UserId == playerId);
            var allCards = _db.Cards.ToList();

            foreach (var paquet in allPaquets)
            {
                var initialMoney = player.Money;

                // Act
                var result = _paquetsService.OpenPack(paquet, allCards, playerId);

                // Assert
                Assert.AreEqual(paquet.NbCartes, result.Count, $"Le joueur doit recevoir exactement {paquet.NbCartes} cartes pour le paquet {paquet.Name}.");

                var playerMoneyAfterPurchase = _db.Players.First(p => p.UserId == playerId).Money;
                Assert.AreEqual(initialMoney - paquet.Cost, playerMoneyAfterPurchase, $"Le montant déduit de l'argent du joueur doit correspondre au coût du paquet {paquet.Name}.");

            }

        }

    }
}