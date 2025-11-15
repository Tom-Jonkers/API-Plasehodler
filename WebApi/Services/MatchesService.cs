using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models.Models;
using Models.Models.Dtos;
using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Data;
using Super_Cartes_Infinies.Models;
using Super_Cartes_Infinies.Models.Dtos;
using WebApi.Models;
using WebApi.Services;

namespace Super_Cartes_Infinies.Services
{
	public class MatchesService
    {
        private WaitingUserService _waitingUserService;
        private PlayersService _playersService;
        private CardsService _cardsService;
        private MatchConfigurationService _matchConfigurationService;
        private DecksService _decksService;
        private ApplicationDbContext _dbContext;
        private MatchmakingService _matchmakingService;

        public MatchesService(ApplicationDbContext context, WaitingUserService waitingUserService, PlayersService playersService, 
                              CardsService cardsService, MatchConfigurationService matchConfigurationService, DecksService decksService,
                              MatchmakingService matchmakingService)
        {
            _dbContext = context;
            _waitingUserService = waitingUserService;
            _playersService = playersService;
            _cardsService = cardsService;
            _matchConfigurationService = matchConfigurationService;
            _decksService = decksService;
            _matchmakingService = matchmakingService;
        }

        // Cette fonction est assez flexible car elle peut simplement être appeler lorsqu'un user veut jouer un match
        // Si le user a déjà un match en cours (Un match qui n'est pas terminé), on lui retourne l'information pour ce match
        // Sinon on utilise le WaitingUserService pour essayer de trouver un autre user ou nous mettre en attente
        public async Task<JoiningMatchData?> JoinMatch(string userId, string? connectionId, int? specificMatchId)
        {
            // Vérifier si le match n'a pas déjà été démarré (de façon plus générale, retourner un match courrant si le joueur y participe)
            IEnumerable<Match> matches = _dbContext.Matches.Where(m => m.IsMatchCompleted == false && (m.UserAId == userId || m.UserBId == userId));

            if(matches.Count() > 1)
            {
                throw new Exception("A player should never be playing 2 matches at the same time!");
            }

            Match? match = null;
            Player? playerA = null;
            Player? playerB = null;
            string otherPlayerConnectionId = null;

            // Le joueur est dans un match en cours
            if (matches.Count() == 1)
            {
                match = matches.First();
                if(specificMatchId != null && specificMatchId != match.Id )
                {
                    match = null;
                }
                else
                {
                    playerA = _playersService.GetPlayerFromUserId(match.UserAId);
                    playerB = _playersService.GetPlayerFromUserId(match.UserBId);
                }

                if (match != null)
                {
                    return new JoiningMatchData
                    {
                        Match = match,
                        PlayerA = playerA!,
                        PlayerB = playerB!,
                        OtherPlayerConnectionId = otherPlayerConnectionId,
                        IsStarted = otherPlayerConnectionId == null
                    };
                }
            }
            // Si on veut rejoindre un match en particulier, on ne se met pas en file
            else if(specificMatchId == null)
            {
                //UsersReadyForAMatch? pairOfUsers = await _waitingUserService.LookForWaitingUser(userId, connectionId);
                Player joiningPlayer = _playersService.GetPlayerFromUserId(userId);
                _matchmakingService.AddUnique(new PlayerInfo(userId, connectionId, joiningPlayer.ELO));



                //if (pairOfUsers != null)
                //{
                //    playerA = _playersService.GetPlayerFromUserId(pairOfUsers.UserAId);
                //    playerB = _playersService.GetPlayerFromUserId(pairOfUsers.UserBId);

                //    //Création d'un nouveau match
                //    var playerACards = await _decksService.GetCurrentDeckCards(playerA.Id);
                //    var playerBCards = await _decksService.GetCurrentDeckCards(playerB.Id);

                //    match = new Match(playerA, playerB, playerACards, playerBCards);
                //    otherPlayerConnectionId = pairOfUsers.UserAConnectionId;

                //    _dbContext.Update(match);
                //    _dbContext.SaveChanges();
                //}
            }

            

            return null;
        }

        public async Task<JoiningMatchData?> SpectateMatch(string userId, string? connectionId, int? specificMatchId)
        {
            // Vérifier si le match n'a pas déjà été démarré (de façon plus générale, retourner un match courrant si le joueur y participe)
            Match match = _dbContext.Matches.SingleOrDefault(m => m.IsMatchCompleted == false && (m.UserAId != userId &&  m.UserBId != userId));

            //IEnumerable<Match> matches = _dbContext.Matches.Where(m => m.IsMatchCompleted == false && (m.UserAId == userId || m.UserBId == userId));

            if (match == null)
            {
                throw new Exception("Ce match n'existe pas!");
            }

            Player? playerA = null;
            Player? playerB = null;
            string otherPlayerConnectionId = null;


            playerA = _playersService.GetPlayerFromUserId(match.UserAId);
            playerB = _playersService.GetPlayerFromUserId(match.UserBId);

            var playerACards = await _decksService.GetCurrentDeckCards(playerA.Id);
            var playerBCards = await _decksService.GetCurrentDeckCards(playerB.Id);


            
                return new JoiningMatchData
                {
                    Match = match,
                    PlayerA = playerA!,
                    PlayerB = playerB!,
                    OtherPlayerConnectionId = otherPlayerConnectionId,
                    IsStarted = otherPlayerConnectionId == null
                };

            return null;
        }

        public async Task<bool> StopJoiningMatch(string userId)
        {
            bool stoppedWaiting = _matchmakingService.RemoveWithUserID(userId);

            return stoppedWaiting;
        }

        // L'action retourne le json de l'event de création de match (StartMatchEvent)
        public async Task<StartMatchEvent> StartMatch(string currentUserId, Match match)
        {
            if ((match.UserAId == currentUserId) != match.IsPlayerATurn)
                throw new Exception("Ce n'est pas le tour de ce joueur");

            MatchPlayerData currentPlayerData;
            MatchPlayerData opposingPlayerData;

            if (match.UserAId == currentUserId)
            {
                currentPlayerData = match.PlayerDataA;
                opposingPlayerData = match.PlayerDataB;
            }
            else
            {
                currentPlayerData = match.PlayerDataB;
                opposingPlayerData = match.PlayerDataA;
            }

            int nbCardsToDraw = _matchConfigurationService.GetNbCardsToDraw();
            int nbManaPerTurn = _matchConfigurationService.GetNbManaPerTurn();
            var startMatchEvent = new StartMatchEvent(match, currentPlayerData, opposingPlayerData, nbCardsToDraw, nbManaPerTurn);
            
            await _dbContext.SaveChangesAsync();

            return startMatchEvent;
        }

        public async Task<PlayerEndTurnEvent> EndTurn(string userId, int matchId)
        {
            Match? match = await _dbContext.Matches.FindAsync(matchId);

            if (match == null)
                throw new Exception("Impossible de trouver le match");

            if (match.IsMatchCompleted)
                throw new Exception("Le match est déjà terminé");

            if (match.UserAId != userId && match.UserBId != userId)
                throw new Exception("Le joueur n'est pas dans ce match");

            if ((match.UserAId == userId) != match.IsPlayerATurn)
                throw new Exception("Ce n'est pas le tour de ce joueur");

            MatchPlayerData currentPlayerData;
            MatchPlayerData opposingPlayerData;

            if (match.UserAId == userId)
            {
                currentPlayerData = match.PlayerDataA;
                opposingPlayerData = match.PlayerDataB;
            }
            else
            {
                currentPlayerData = match.PlayerDataB;
                opposingPlayerData = match.PlayerDataA;
            }

            int nbManaPerTurn = _matchConfigurationService.GetNbManaPerTurn();

            GameConfig gameConfig = await _dbContext.GameConfigs.FirstOrDefaultAsync();
            if (gameConfig == null)
            {
                throw new Exception("Configuration de jeu introuvable");
            }

            var playerEndTurnEvent = new PlayerEndTurnEvent(match, currentPlayerData, opposingPlayerData, nbManaPerTurn, gameConfig);

            await _dbContext.SaveChangesAsync();
            
            return playerEndTurnEvent;
        }

        public async Task<SurrenderEvent> Surrender(string userId, int matchId)
        {
            Match? match = await _dbContext.Matches.FindAsync(matchId);

            if (match == null)
                throw new Exception("Impossible de trouver le match");

            if (match.IsMatchCompleted)
                throw new Exception("Le match est déjà terminé");

            if (match.UserAId != userId && match.UserBId != userId)
                throw new Exception("Le joueur n'est pas dans ce match");

            MatchPlayerData currentPlayerData;
            MatchPlayerData opposingPlayerData;

            if (match.UserAId == userId)
            {
                currentPlayerData = match.PlayerDataA;
                opposingPlayerData = match.PlayerDataB;
            }
            else
            {
                currentPlayerData = match.PlayerDataB;
                opposingPlayerData = match.PlayerDataA;
            }

            GameConfig gameConfig = await _dbContext.GameConfigs.FirstOrDefaultAsync();
            if (gameConfig == null)
            {
                throw new Exception("Configuration de jeu introuvable");
            }

            var surrenderEvent = new SurrenderEvent(match, currentPlayerData, opposingPlayerData, gameConfig);

            await _dbContext.SaveChangesAsync();

            return surrenderEvent;
        }

        public async Task<PlayCardEvent?> PlayCard(string userId, int matchId, int playableCardId)
        {
            Match? match = await _dbContext.Matches.FindAsync(matchId);

            if (match == null)
                throw new Exception("Impossible de trouver le match");

            if (match.IsMatchCompleted)
                throw new Exception("Le match est déjà terminé");

            if (match.UserAId != userId && match.UserBId != userId)
                throw new Exception("Le joueur n'est pas dans ce match");


            MatchPlayerData currentPlayerData;
            MatchPlayerData opposingPlayerData;

            if (match.UserAId == userId)
            {
                currentPlayerData = match.PlayerDataA;
                opposingPlayerData = match.PlayerDataB;
                if (!match.IsPlayerATurn)
                    throw new Exception("Ce n'est pas votre tour");
            }
            else
            {
                currentPlayerData = match.PlayerDataB;
                opposingPlayerData = match.PlayerDataA;
                if (match.IsPlayerATurn)
                    throw new Exception("Ce n'est pas votre tour");
            }

            var playableCard = currentPlayerData.Hand.Where(c => c.Id == playableCardId).Single();

            if (currentPlayerData.Mana >= playableCard.Card.Cost)
            {
                var playCardEvent = new PlayCardEvent(currentPlayerData, opposingPlayerData, playableCardId);

                await _dbContext.SaveChangesAsync();

                return playCardEvent;
                
            }
            else
            {
                throw new Exception("Mana inssufisant");
            }
                
        }

        public IEnumerable<MatchDTO> GetAllMatches()
        {
            List<MatchDTO> matchDTOs = new List<MatchDTO>();
            
                foreach (Match match in _dbContext.Matches.ToList().Where(m => !m.IsMatchCompleted))
                {
                    matchDTOs.Add(new MatchDTO { PlayerNameA = match.PlayerDataA.Player.Name, UserIdA = match.PlayerDataA.Player.UserId, PlayerNameB = match.PlayerDataB.Player.Name, UserIdB = match.PlayerDataB.Player.UserId, Id = match.Id });

                }
            return matchDTOs;
        }
    }
}
