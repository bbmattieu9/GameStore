using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Models;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
        new (1, 
            "Street Fighter II", 
            "Fighting", 
            19.99M, 
            new DateOnly(1992, 7, 15)),
        new  (
            2,
            "Elden Ring",
            "Platformer",
            19.99M,
            new DateOnly(1991, 9, 12)
        ),
        new (
            3,
            "Super Mario Wonder",
            "RPG",
            69.30M,
            new DateOnly(1995, 9,  20))
    ];
    
    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        
        group.MapGet("/", () => Results.Ok(games));

        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = new Game
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDetailsDto gameDto = new GameDetailsDto(
                game.GameId,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var removedCount = games.RemoveAll(game => game.Id == id);
            return removedCount > 0 ? Results.NoContent() : Results.NotFound();
        });
    }
}