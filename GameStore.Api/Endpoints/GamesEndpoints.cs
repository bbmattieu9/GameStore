using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    
    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        
        group.MapGet("/", async(GameStoreContext dbContext) => 
        
            await dbContext.Games
            .Include(game => game.Genre)
            .Select(game => new GameSummaryDto(
                game.GameId,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            ))
            .AsNoTracking()
            .ToListAsync());



        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is null ?  Results.NotFound() : Results.Ok(new GameDetailsDto(
                game.GameId,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            ));
        }).WithName(GetGameEndpointName);



        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = new Game
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new GameDetailsDto(
                game.GameId,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });



        group.MapPut("/{id}", async (
         int id,
         UpdateGameDto updateGame,
         GameStoreContext dbContext) =>
        {
           var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updateGame.Name;
            existingGame.GenreId = updateGame.GenreId;
            existingGame.Price = updateGame.Price;
            existingGame.ReleaseDate = updateGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.GameId == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}