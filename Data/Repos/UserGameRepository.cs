using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Logic.Dtos;
using Logic.Mappers;

namespace Logic.Repos
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly string _connectionString;

        public UserGameRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<UserGame> GetAllUserGames(int userId)
        {
            var userGames = new List<UserGame>();
            const string sql = @"SELECT
                                    ug.*, 
                                    g.Title,
                                    g.CoverImageUrl,
                                    gen.Name AS GenreName, 
                                    dev.Name AS DeveloperName, 
                                    pub.Name AS PublisherName
                                FROM UserGame ug
                                INNER JOIN Game g ON ug.GameId = g.Id
                                INNER JOIN Genre gen ON g.GenreId = gen.Id
                                INNER JOIN Developer dev ON g.DeveloperId = dev.Id
                                INNER JOIN Publisher pub ON g.PublisherId = pub.Id
                                WHERE ug.UserId = @UserId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var dto = new UserGameDto
                {
                    UserId = (int)reader["UserId"],
                    GameId = (int)reader["GameId"],
                    Status = (byte)reader["Status"],
                    Platform = (byte)reader["Platform"],
                    PricePaid = (decimal)reader["PricePaid"],
                    PurchacedAt = (DateTime)reader["PurchacedAt"],
                    AddedAt = (DateTime)reader["AddedAt"],
                    HoursPlayed = (int)reader["HoursPlayed"],
                    Notes = (string)reader["Notes"],
                    Game = new GameDto
                    {
                        Id = (int)reader["GameId"],
                        Title = (string)reader["Title"],
                        GenreName = (string)reader["GenreName"],
                        DeveloperName = (string)reader["DeveloperName"],
                        PublisherName = (string)reader["PublisherName"],
                        CoverImageUrl = (string)reader["CoverImageUrl"]
                    }
                };
                userGames.Add(UserGameMapper.ToEntity(dto));
            }
            reader.Close();
            conn.Close();
            return userGames;
        }

        public UserGame? GetUserGame(int userId, int gameId)
        {
            const string sql =@"SELECT 
                                    ug.*, 
                                    g.Title,
                                    gen.Name AS GenreName, 
                                    dev.Name AS DeveloperName, 
                                    pub.Name AS PublisherName
                                FROM UserGame ug
                                INNER JOIN Game g ON ug.GameId = g.Id
                                INNER JOIN Genre gen ON g.GenreId = gen.Id
                                INNER JOIN Developer dev ON g.DeveloperId = dev.Id
                                INNER JOIN Publisher pub ON g.PublisherId = pub.Id
                                WHERE ug.UserId = @UserId AND ug.GameId = GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var dto = new UserGameDto
                {
                    UserId = (int)reader["UserId"],
                    GameId = (int)reader["GameId"],
                    Status = (byte)reader["Status"],
                    Platform = (byte)reader["Platform"],
                    PricePaid = (decimal)reader["PricePaid"],
                    PurchacedAt = (DateTime)reader["PurchacedAt"],
                    AddedAt = (DateTime)reader["AddedAt"],
                    HoursPlayed = (int)reader["HoursPlayed"],
                    Notes = (string)reader["Notes"],
                    Game = new GameDto
                    {
                        Id = (int)reader["GameId"],
                        Title = (string)reader["Title"],
                        GenreName = (string)reader["GenreName"],
                        DeveloperName = (string)reader["DeveloperName"],
                        PublisherName = (string)reader["PublisherName"]
                    }
                };
                reader.Close();
                conn.Close();
                return UserGameMapper.ToEntity(dto);
            }
            reader.Close();
            conn.Close();
            return null;
        }

        public bool UserGameExists(int userId, int gameId)
        {
            const string sql = "SELECT COUNT(1) FROM UserGame WHERE UserId = @UserId AND GameId = @GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            conn.Open();
            var count = (int)cmd.ExecuteScalar();
            conn.Close();

            // if count 1, the game exists. if count is 0 , it does not exist.
            return count > 0;
        }
        public void CreateUserGame(UserGame userGame)
        {
            const string sql = @"INSERT INTO UserGame (UserId, GameId, Status, Platform, PricePaid, PurchacedAt, AddedAt, HoursPlayed, Notes)
                                 VALUES (@UserId, @GameId, @Status, @Platform, @PricePaid, @PurchacedAt, @AddedAt, @HoursPlayed, @Notes)";
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userGame.UserId);
            cmd.Parameters.AddWithValue("@GameId", userGame.GameId);
            cmd.Parameters.AddWithValue("@Status", (int)userGame.Status);
            cmd.Parameters.AddWithValue("@Platform", (int)userGame.Platform);
            cmd.Parameters.AddWithValue("@PricePaid", userGame.PricePaid);
            cmd.Parameters.AddWithValue("@PurchacedAt", userGame.PurchacedAt);
            cmd.Parameters.AddWithValue("@AddedAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@HoursPlayed", userGame.HoursPlayed);
            cmd.Parameters.AddWithValue("@Notes", userGame.Notes ?? (object)DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateUserGame(UserGame userGame)
        {
            var UserGameDto = UserGameMapper.ToDto(userGame);

            const string sql = @"UPDATE UserGame 
                                 SET Status = @Status, 
                                     Platform = @Platform, 
                                     PricePaid = @PricePaid, 
                                     PurchacedAt = @PurchacedAt, 
                                     AddedAt = @AddedAt, 
                                     HoursPlayed = @HoursPlayed, 
                                     Notes = @Notes
                                 WHERE UserId = @UserId AND GameId = @GameId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@UserId", UserGameDto.UserId);
            cmd.Parameters.AddWithValue("@GameId", UserGameDto.GameId);
            cmd.Parameters.AddWithValue("@Status", UserGameDto.Status);
            cmd.Parameters.AddWithValue("@Platform", UserGameDto.Platform);
            cmd.Parameters.AddWithValue("@PricePaid", UserGameDto.PricePaid);
            cmd.Parameters.AddWithValue("@PurchacedAt", UserGameDto.PurchacedAt);
            cmd.Parameters.AddWithValue("@AddedAt", UserGameDto.AddedAt);
            cmd.Parameters.AddWithValue("@HoursPlayed", UserGameDto.HoursPlayed);
            cmd.Parameters.AddWithValue("@Notes", (object?)UserGameDto.Notes ?? DBNull.Value);


            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteUserGame(UserGame userGame)
        {
            const string sql = "DELETE FROM UserGame WHERE UserId = @UserId AND GameId = @GameId";
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userGame.UserId);
            cmd.Parameters.AddWithValue("@GameId", userGame.GameId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
