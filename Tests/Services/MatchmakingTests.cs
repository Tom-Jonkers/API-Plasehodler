using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Hubs;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace Tests.Services
{
    


    [TestClass]
    public class MatchmakingTests
    {
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IHubContext<MatchHub>> _hubContextMock;
        private readonly MatchmakingService _matchmakingService;

        public MatchmakingTests()
        {
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _hubContextMock = new Mock<IHubContext<MatchHub>>();
            _matchmakingService = new MatchmakingService(_serviceScopeFactoryMock.Object, _hubContextMock.Object);
        }

        [TestMethod]
        public void GeneratePairsWith2closePlayers()
        {
            // Arrange
            var playerInfo1 = new PlayerInfo("user1", "conn1", 1990);
            playerInfo1.attente = 21;
            var playerInfo2 = new PlayerInfo("user2", "conn2", 2010);
            playerInfo2.attente = 1;

            _matchmakingService.listeOriginale.Add(playerInfo1);
            _matchmakingService.listeOriginale.Add(playerInfo2);

            // Act
            var pairs = _matchmakingService.GeneratePairs(_matchmakingService.listeOriginale);

            // Assert
            Assert.AreEqual(1, pairs.Count, "There should be exactly 1 pair.");
        }

        [TestMethod]
        public void GeneratePairsWith2farPlayers()
        {
            // Arrange
            var playerInfo1 = new PlayerInfo("user1", "conn1", 500);
            playerInfo1.attente = 60;
            var playerInfo2 = new PlayerInfo("user2", "conn2", 1500);
            playerInfo2.attente = 60;

            _matchmakingService.listeOriginale.Add(playerInfo1);
            _matchmakingService.listeOriginale.Add(playerInfo2);

            // Act
            var pairs = _matchmakingService.GeneratePairs(_matchmakingService.listeOriginale);

            // Assert
            Assert.AreEqual(0, pairs.Count, "There should be exactly 0 pairs.");
        }

        [TestMethod]
        public void GeneratePairsWith2closePairsAnd1farPair()
        {
            // Arrange
            var smartPlayerInfo1 = new PlayerInfo("user1", "conn1", 1990);
            smartPlayerInfo1.attente = 21;
            var smartPlayerInfo2 = new PlayerInfo("user2", "conn2", 2010);
            smartPlayerInfo2.attente = 1;

            var dumbPlayerInfo1 = new PlayerInfo("user3", "conn3", 90);
            dumbPlayerInfo1.attente = 40;
            var dumbPlayerInfo2 = new PlayerInfo("user4", "conn4", 110);
            dumbPlayerInfo2.attente = 40;

            var smartPlayerInfo3 = new PlayerInfo("user6", "conn6", 1500);
            smartPlayerInfo3.attente = 60;
            var dumbPlayerInfo3 = new PlayerInfo("userrandom", "conn5", 500);
            dumbPlayerInfo3.attente = 60;

            _matchmakingService.listeOriginale.Add(smartPlayerInfo1);
            _matchmakingService.listeOriginale.Add(smartPlayerInfo2);
            _matchmakingService.listeOriginale.Add(dumbPlayerInfo1);
            _matchmakingService.listeOriginale.Add(dumbPlayerInfo2);
            _matchmakingService.listeOriginale.Add(smartPlayerInfo3);
            _matchmakingService.listeOriginale.Add(dumbPlayerInfo3);

            // Act
            var pairs = _matchmakingService.GeneratePairs(_matchmakingService.listeOriginale);

            // Assert
            Assert.AreEqual(2, pairs.Count, "There should be exactly 2 pairs.");

            // Check if smart players are paired together
            var smartPair = pairs.FirstOrDefault(p =>
                (p.Player1 == smartPlayerInfo1 && p.Player2 == smartPlayerInfo2) ||
                (p.Player1 == smartPlayerInfo2 && p.Player2 == smartPlayerInfo1));
            Assert.IsNotNull(smartPair, "Smart players should be paired together.");

            // Check if dumb players are paired together
            var dumbPair = pairs.FirstOrDefault(p =>
                (p.Player1 == dumbPlayerInfo1 && p.Player2 == dumbPlayerInfo2) ||
                (p.Player1 == dumbPlayerInfo2 && p.Player2 == dumbPlayerInfo1));
            Assert.IsNotNull(dumbPair, "Dumb players should be paired together.");

            // Assert that dumbPlayer3 and smartPlayer3 are not matched together
            var areDumb3AndSmart3Paired = pairs.Any(p =>
                (p.Player1 == dumbPlayerInfo3 && p.Player2 == smartPlayerInfo3) ||
                (p.Player1 == smartPlayerInfo3 && p.Player2 == dumbPlayerInfo3));
            Assert.IsFalse(areDumb3AndSmart3Paired, "dumbPlayer3 and smartPlayer3 should not be matched together.");

        }

        [TestMethod]
        public void AddUniqueFonctionne()
        {
            // Arrange
            var playerInfo = new PlayerInfo("user1", "conn1", 1000);

            // Act
            _matchmakingService.AddUnique(playerInfo);

            // Assert
            Assert.IsTrue(_matchmakingService.listeOriginale.Contains(playerInfo));
        }

        [TestMethod]
        public void AddUniqueAssertNoDupes()
        {
            // Arrange
            var playerInfo1 = new PlayerInfo("user1", "conn1", 1000);
            var playerInfo2 = new PlayerInfo("user1", "conn2", 1000);
            var playerInfo3 = new PlayerInfo("user2", "conn1", 1000);

            // Act
            _matchmakingService.AddUnique(playerInfo1);
            _matchmakingService.AddUnique(playerInfo2);
            _matchmakingService.AddUnique(playerInfo3);

            // Assert
            Assert.IsTrue(_matchmakingService.listeOriginale.Contains(playerInfo1));
        }

        [TestMethod]
        public void RemoveUserWithID()
        {
            // Arrange
            var playerInfo1 = new PlayerInfo("user1", "conn1", 1000);

            // Act
            _matchmakingService.AddUnique(playerInfo1);
            _matchmakingService.RemoveWithUserID(playerInfo1.UserID);

            // Assert
            Assert.IsTrue(_matchmakingService.listeOriginale.Count == 0);
        }

        

        [TestMethod]
        public async Task WaitTimeIncreases()
        {
            // Arrange
            var player1 = new PlayerInfo("user1", "conn1", 1000) { attente = 0 };
            var player2 = new PlayerInfo("user2", "conn2", 1010) { attente = 0 };
            var player3 = new PlayerInfo("user3", "conn3", 1200) { attente = 0 };

            _matchmakingService.AddUnique(player1);
            _matchmakingService.AddUnique(player2);
            _matchmakingService.AddUnique(player3);

            var initialWaitTime1 = player1.attente;
            var initialWaitTime2 = player2.attente;
            var initialWaitTime3 = player3.attente;

            // Create a CancellationTokenSource with a delay to allow the method to run
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000); // Allow the service to run for 3 seconds

            // Act
            var executeTask = _matchmakingService.StartAsync(cancellationTokenSource.Token);

            // Wait for the cancellation token to trigger
            await Task.Delay(3000); // Wait slightly longer than the cancellation time to ensure the method runs

            // Assert
            Assert.IsTrue(player1.attente > initialWaitTime1, "Player1's wait time should have increased. It is currently at " + player1.attente);
            Assert.IsTrue(player2.attente > initialWaitTime2, "Player2's wait time should have increased. It is currently at " + player2.attente);
            Assert.IsTrue(player3.attente > initialWaitTime3, "Player3's wait time should have increased. It is currently at " + player3.attente);
        }

    }
}
