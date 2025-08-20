using RichbetsResurrected.Entities.Games.Roulette;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Games.Roulette;

namespace RichbetsResurrected.Services.Games.Roulette;

public class RouletteService : IRouletteService
{
    private readonly IRichbetRepository _repository;

    public RouletteService(IRichbetRepository repository, IRouletteGameState gameState)
    {
        _repository = repository;
        GameState = gameState;
    }

    public IRouletteGameState GameState { get; }

    public async Task<RouletteJoinResult> AddPlayerAsync(RoulettePlayer player)
    {
        if (!GameState.CheckIfCanBet())
        {
            return new RouletteJoinResult
            {
                IsSuccess = false,
                Error = new RouletteError {Message = "You cannot bet at this time"},
                Player = player
            };
        }

        if (player.Amount <= 0)
        {
            return new RouletteJoinResult
            {
                IsSuccess = false,
                Error = new RouletteError {Message = "You cannot bet with a negative amount"},
                Player = player
            };
        }

        var points = await _repository.GetPointsFromUserAsync(player.IdentityUserId);

        if (points - player.Amount < 0)
        {
            return new RouletteJoinResult
            {
                IsSuccess = false,
                Error = new RouletteError {Message = "You don't have enough points"},
                Player = player
            };
        }


        await _repository.RemovePointsFromUserAsync(player.IdentityUserId, player.Amount);
        GameState.AddPlayer(player);
        // await SendJoinPlayerToClientsAsync(player);
        return new RouletteJoinResult {IsSuccess = true, Error = null, Player = player};
    }
}
