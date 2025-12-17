using Data.Dtos;
using Data.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Data.Repos
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;

        public GameRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public void CreateGame(Game game)
        {
            GameDto dto = GameDataMapper.ToDto(game);

            const string sql = @"
                INSERT INTO Game (Title, GenreId, ReleaseDate, DeveloperId, PublisherId, Price, PegiAge, Description, CoverImageUrl, CreatedAt, UpdatedAt, IsArchived)
                VALUES (@Title, @GenreId, @ReleaseDate, @DeveloperId, @PublisherId, @Price, @PegiAge, @Description, @CoverImageUrl, @CreatedAt, @UpdatedAt, @IsArchived)";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@GenreId", dto.GenreId);
            cmd.Parameters.AddWithValue("@ReleaseDate", dto.ReleaseDate);
            cmd.Parameters.AddWithValue("@DeveloperId", dto.DeveloperId);
            cmd.Parameters.AddWithValue("@PublisherId", dto.PublisherId);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PegiAge", dto.PegiAge);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@CoverImageUrl", dto.CoverImageUrl);
            cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            cmd.Parameters.AddWithValue("@UpdatedAt", dto.UpdatedAt);
            cmd.Parameters.AddWithValue("@IsArchived", dto.IsArchived);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public IEnumerable<Game> GetAllGames()
        {
            var games = new List<Game>();

            const string sql = "SELECT * FROM Game WHERE IsArchived = 0";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var dto = new GameDto
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    GenreId = (int)reader["GenreId"],
                    ReleaseDate = (DateTime)reader["ReleaseDate"],
                    DeveloperId = (int)reader["DeveloperId"],
                    PublisherId = (int)reader["PublisherId"],
                    Price = (decimal)reader["Price"],
                    PegiAge = (byte)reader["PegiAge"],
                    Description = (string)reader["Description"],
                    CoverImageUrl = (string)reader["CoverImageUrl"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UpdatedAt = (DateTime)reader["UpdatedAt"],
                    IsArchived = (bool)reader["IsArchived"]
                };
                games.Add(GameDataMapper.ToEntity(dto));
            }

            reader.Close();
            conn.Close();

            return games;
        }

        public Game? GetById(int id)
        {
            const string sql = "SELECT * FROM Game WHERE Id = @Id AND IsArchived = 0";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;
            
            var dto = new GameDto
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                GenreId = (int)reader["GenreId"],
                ReleaseDate = (DateTime)reader["ReleaseDate"],
                DeveloperId = (int)reader["DeveloperId"],
                PublisherId = (int)reader["PublisherId"],
                Price = (decimal)reader["Price"],
                PegiAge = (byte)reader["PegiAge"],
                Description = (string)reader["Description"],
                CoverImageUrl = (string)reader["CoverImageUrl"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = (DateTime)reader["UpdatedAt"],
                IsArchived = (bool)reader["IsArchived"]
            };

            reader.Close();
            conn.Close();

            return GameDataMapper.ToEntity(dto);
        }

        public void UpdateGame(Game game)
        {
            GameDto dto = GameDataMapper.ToDto(game);

            const string sql = @"UPDATE Game
                                 SET Title = @Title,GenreId = @GenreId,ReleaseDate = @ReleaseDate,DeveloperId = @DeveloperId,PublisherId = @PublisherId,Price = @Price,PegiAge = @PegiAge,Description = @Description,CoverImageUrl = @CoverImageUrl,UpdatedAt = @UpdatedAt,IsArchived = @IsArchived
                                 WHERE Id = @Id";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", dto.Id);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@GenreId", dto.GenreId);
            cmd.Parameters.AddWithValue("@ReleaseDate", dto.ReleaseDate);
            cmd.Parameters.AddWithValue("@DeveloperId", dto.DeveloperId);
            cmd.Parameters.AddWithValue("@PublisherId", dto.PublisherId);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PegiAge", dto.PegiAge);
            cmd.Parameters.AddWithValue("@Description", dto.Description);
            cmd.Parameters.AddWithValue("@CoverImageUrl", dto.CoverImageUrl);
            cmd.Parameters.AddWithValue("@UpdatedAt", dto.UpdatedAt);
            cmd.Parameters.AddWithValue("@IsArchived", dto.IsArchived);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void DeleteGame(int id)
        {
            const string sql = @"DELETE FROM Game WHERE Id = @Id";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
