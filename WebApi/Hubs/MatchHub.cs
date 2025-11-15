using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models.Models.Dtos;
using Super_Cartes_Infinies.Combat;
using Super_Cartes_Infinies.Services;
using System.Security.Claims;
using WebApi.Services;
using WebApi.Models;

namespace Super_Cartes_Infinies.Hubs;

[Authorize]
public class MatchHub : Hub
{
	private readonly MatchesService _matchesService;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly MatchmakingService _matchmakingService;

	public MatchHub(MatchesService matchesService, UserManager<IdentityUser> userManager)
	{
		_matchesService = matchesService;
		_userManager = userManager;
	}

	public async Task JoinMatch()
	{
		var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		//_matchmakingService.listOriginale.Add(new PlayerInfo(user.Id, Context.ConnectionId));

		var matchData = await _matchesService.JoinMatch(user.Id, Context.ConnectionId, null);

		if (matchData != null)
		{
			if (matchData.IsStarted)
			{
				var groupName = matchData.Match.Id.ToString();

				await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

				await Clients.Caller.SendAsync("JoiningMatchData", matchData);
			}
			//else
			//{
			//	var groupName = matchData.Match.Id.ToString();

			//	await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
			//	await Groups.AddToGroupAsync(matchData.OtherPlayerConnectionId, groupName);

			//	await Clients.Group(groupName).SendAsync("JoiningMatchData", matchData);

			//	var startMatchEvent = await _matchesService.StartMatch(user.Id, matchData.Match);

			//	//await Clients.Group(groupName).SendAsync(startMatchEvent.EventType, startMatchEvent);
			//	await Clients.Group(groupName).SendAsync("Event", startMatchEvent);
			//}
		}
	}

    public async Task SpectateMatch(int matchId)
    {
        var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var matchData = await _matchesService.SpectateMatch(user.Id, Context.ConnectionId, null);

        if (matchData != null)
        {
            if (matchData.IsStarted)
            {
                var groupName = matchData.Match.Id.ToString();
                var groupNameSpec = matchData.Match.Id.ToString() + " Spectator";

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupNameSpec);

                await Clients.Caller.SendAsync("JoiningMatchData", matchData);
            }
        }

    }

    public async Task StopJoiningMatch()
	{
        var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _matchesService.StopJoiningMatch(user.Id);
	}

	public async Task EndTurn(int matchId)
	{
		var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		var playerEndTurnEvent = await _matchesService.EndTurn(user.Id, matchId);

        //await Clients.Group(matchId.ToString()).SendAsync(playerEndTurnEvent.EventType, playerEndTurnEvent);
        await Clients.Group(matchId.ToString()).SendAsync("Event", playerEndTurnEvent);
    }

	public async Task Surrender(int matchId)
	{
		var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		var surrenderEvent = await _matchesService.Surrender(user.Id, matchId);

		//await Clients.Group(matchId.ToString()).SendAsync(surrenderEvent.EventType, surrenderEvent);
        await Clients.Group(matchId.ToString()).SendAsync("Event", surrenderEvent);
    }

	public async Task PlayCard(int matchId, int playableCardID)
	{
        var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

		var playCardEvent = await _matchesService.PlayCard(user.Id, matchId, playableCardID);

        //await Clients.Group(matchId.ToString()).SendAsync(playCardEvent.EventType, playCardEvent);
        await Clients.Group(matchId.ToString()).SendAsync("Event", playCardEvent);
    }

	public async Task SendMessage(int matchId, string message, bool isSpectator)
	{
        var user = await _userManager.FindByIdAsync(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
		var dto = new ChatDTO
			      {
					Pseudo = user.UserName,
					Message = message,
					isSpectator = isSpectator
				  };

		await Clients.Group(matchId.ToString()).SendAsync("Message", dto);
    }

	
}
