using Logic.Entities;

namespace Logic.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetAllGames();
        Game? GetGame(Game game);
        void CreateGame(Game game);
        void UpdateGame(Game game);
        void ArchiveGame(Game game);
    }
}
