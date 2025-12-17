using Data.Dtos;
using Data.Mappers;
using Logic.Entities;
using Logic.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repos
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly string _connectionString;

        public DeveloperRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public IEnumerable<Developer> GetAllDevelopers ()
        {
            var developers = new List<Developer>();

            const string sql = "SELECT * FROM Developer";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);

            conn.Open();
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var dto = new DeveloperDto
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                };
                developers.Add(DeveloperDataMapper.ToEntity(dto));
            }

            reader.Close();
            conn.Close();

            return developers;
        }
    }
}
