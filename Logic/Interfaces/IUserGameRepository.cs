using Logic.Dtos;

namespace Logic.Interfaces
{
    public interface IUserGameRepository
    {
        IEnumerable<UserGameDto> GetAllUserGames(int userId);
        UserGameDto? GetUserGame(int userId, int gameId);
        bool UserGameExists(int userId, int gameId);
        void CreateUserGame(UserGameDto userGame);
        void UpdateUserGame(UserGameDto userGame);
        void DeleteUserGame(UserGameDto userGame);
    }
}
