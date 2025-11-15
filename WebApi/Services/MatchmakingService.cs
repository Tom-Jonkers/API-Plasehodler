using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Hubs;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Models.Dtos;
using Super_Cartes_Infinies.Services;
using WebApi.Models;

namespace WebApi.Services
{
    public class MatchmakingService : BackgroundService
    {
        public const int DELAY = 1000;
        public const double CONSTANTE = 1.0;
        public List<PlayerInfo> listeOriginale;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<MatchHub> _matchHub;



        public MatchmakingService(IServiceScopeFactory serviceScopeFactory, IHubContext<MatchHub> matchHub)
        {
            _serviceScopeFactory = serviceScopeFactory;
            listeOriginale = new List<PlayerInfo>();
            _matchHub = matchHub;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(DELAY, stoppingToken);
                    var copy = new List<PlayerInfo>(listeOriginale);

                    List<PairOfPlayers> pairs = GeneratePairs(copy);
                    
                    foreach (PairOfPlayers pair in pairs)
                    {
                        if (pair != null)
                        {
                            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                            {
                                PlayersService _playersService = scope.ServiceProvider.GetRequiredService<PlayersService>();
                                DecksService _decksService = scope.ServiceProvider.GetRequiredService<DecksService>();
                                ApplicationDbContext _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                                MatchesService _matchesService = scope.ServiceProvider.GetRequiredService<MatchesService>();


                                Player playerA = _playersService.GetPlayerFromUserId(pair.Player1.UserID);
                                Player playerB = _playersService.GetPlayerFromUserId(pair.Player2.UserID);

                                // Création d'un nouveau match
                                var playerACards = await _decksService.GetCurrentDeckCards(playerA.Id);
                                var playerBCards = await _decksService.GetCurrentDeckCards(playerB.Id);
                                Match match = new Match(playerA, playerB, playerACards, playerBCards);
                                string otherPlayerConnectionId = pair.Player1.ConnexionID;

                                _dbContext.Update(match);
                                _dbContext.SaveChanges();

                                if (match != null)
                                {
                                    JoiningMatchData matchData = new JoiningMatchData 
                                    {
                                        Match = match,
                                        PlayerA = playerA!,
                                        PlayerB = playerB!,
                                        OtherPlayerConnectionId = otherPlayerConnectionId,
                                        IsStarted = otherPlayerConnectionId == null
                                    };

                                    var groupName = matchData.Match.Id.ToString();

                                    await _matchHub.Groups.AddToGroupAsync(pair.Player2.ConnexionID, groupName); // Ou player 2 je sais puuu!!!!
                                    await _matchHub.Groups.AddToGroupAsync(matchData.OtherPlayerConnectionId, groupName);

                                    await _matchHub.Clients.Group(groupName).SendAsync("JoiningMatchData", matchData);

                                    var startMatchEvent = await _matchesService.StartMatch(pair.Player2.UserID, matchData.Match); // Ou Player 2 je sais encore puuu!!!!

                                    //await Clients.Group(groupName).SendAsync(startMatchEvent.EventType, startMatchEvent);
                                    await _matchHub.Clients.Group(groupName).SendAsync("Event", startMatchEvent);

                                    listeOriginale.Remove(pair.Player1);
                                    listeOriginale.Remove(pair.Player2);
                                }


                            }
                        }
                    }

                    foreach (PlayerInfo playerInfo in listeOriginale)
                    {
                        playerInfo.attente++;
                    }
                }   
                
        }

  

        // Passer une COPIE de l'information sur les players (Car on va retirer les éléments de la liste, même si le player n'est pas mis dans une paire)
        public List<PairOfPlayers> GeneratePairs(List<PlayerInfo> playerInfos)
        {
            List<PairOfPlayers> pairs = new List<PairOfPlayers>();

            // Tant qu'il y a des joueurs à mettre en pair
            while (playerInfos.Count > 0)
            {
                PlayerInfo playerInfo = playerInfos[0];
                playerInfos.RemoveAt(0); // Remove first player from the list

                int smallestELODifference = int.MaxValue;
                int index = -1;

                // Recherche du joueur avec la différence d'ELO la plus faible
                for (int i = 0; i < playerInfos.Count; i++)
                {
                    PlayerInfo pi = playerInfos[i];
                    int difference = Math.Abs(pi.ELO - playerInfo.ELO);

                    // Check si la différence respecte la condition d'attente
                    if (difference < playerInfo.attente * CONSTANTE)
                    {
                        if (difference < smallestELODifference)
                        {
                            smallestELODifference = difference;
                            index = i;
                        }
                    }
                }

                // Si une paire est trouvée
                if (index >= 0)
                {
                    PlayerInfo playerInfo2 = playerInfos[index];
                    playerInfos.RemoveAt(index); // Remove paired player from the list
                    pairs.Add(new PairOfPlayers(playerInfo, playerInfo2));
                }
                // Sinon, on a retiré l'élément de la liste, on passe au suivant
            }

            return pairs;
        }

        public bool RemoveWithUserID(string userID)
        {
            var player = listeOriginale.Where(x => x.UserID == userID).SingleOrDefault();

            bool isRemoved = false;
            if (player != null)
            {
                listeOriginale.Remove(player);
                isRemoved = true;
            }
            return isRemoved;
        }

        public void AddUnique(PlayerInfo playerInfo)
        {
            if (!listeOriginale.Any(x => x.ConnexionID == playerInfo.ConnexionID || x.UserID == playerInfo.UserID))
            {
                listeOriginale.Add(playerInfo);
            }
        }
    }

    

    
}
