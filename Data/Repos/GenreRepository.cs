using Logic.Dtos;
using Logic.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repos
{
    public class GenreRepository : IGenreRepository
    {
        private readonly string _connectionString;

        public GenreRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<GenreDto> GetAllGenres()
        {
            var genres = new List<GenreDto>();

            const string sql = "SELECT * FROM Genre";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var dto = new GenreDto
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                };
                genres.Add(dto);
            }

            reader.Close();
            conn.Close();

            return genres;
        }
    }
}
