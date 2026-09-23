using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// app.MapGet("/", () => "Hello World!");

app.MapGet("/games", () => Results.Ok(games));

app.Run();


