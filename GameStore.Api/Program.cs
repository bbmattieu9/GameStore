

using GameStore.Api.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();
app.MapGamesEndpoint();

app.Run();


