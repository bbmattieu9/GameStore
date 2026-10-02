namespace GameStore.Api.Endpoints;

public class Game
{
    public int GameId { get; set; }
    public required string Name { get; set; }
    public Genre? Genre  {get; set;}
    public int GenreId { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
}