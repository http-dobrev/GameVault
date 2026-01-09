using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<GameDto> GetAllGames();
        GameDto? GetGame(int id);
        bool GameExists(string title);
        void CreateGame(GameDto dto);
        void UpdateGame(GameDto dto);
        void ArchiveGame(int id);
    }
}
