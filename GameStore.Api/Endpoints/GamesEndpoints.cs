using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

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
        app.MapGet("/games", () => Results.Ok(games));

        app.MapGet("/games/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(game => game.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        app.MapPost("/games", (CreateGameDto newGame) =>
        {
            var game = new GameDto(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        app.MapPut("/games/{id}", (int id, UpdateGameDto updateGame) =>
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

        app.MapDelete("/games/{id}", (int id) =>
        {
            var removedCount = games.RemoveAll(game => game.Id == id);
            return removedCount > 0 ? Results.NoContent() : Results.NotFound();
        });
    }
}