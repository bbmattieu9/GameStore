using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/games", () => Results.Ok(games));

// Get a game by id
{
    app.MapGet("/games/{id}", (int id) => {
        var game = games.FirstOrDefault(g => g.Id == id);
        return game != null ? Results.Ok(game) : Results.NotFound();
    }).WithName(GetGameEndpointName);
}

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new GameDto(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);
    
    games.Add(game);
    
    return Results.CreatedAtRoute(GetGameEndpointName, new {  id = game.Id }, game);
});

// PUT /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(g => g.Id == id);
    if (index == -1)
    {
        return Results.NotFound();
    }
    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
        );

    return Results.NoContent();
});

// DELETE /games/
app.MapDelete("/games/{id}", (int id) =>
{
    games.Remove(games.FirstOrDefault(g => g.Id == id));

    return Results.NoContent();
});

app.Run();


