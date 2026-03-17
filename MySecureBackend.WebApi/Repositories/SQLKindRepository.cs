using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLKindRepository : IKind
    {
        private readonly string sqlConnectionString;

        public SQLKindRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Kind kind)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Kind] (KindID, BehandelingID, Naam, Leeftijd) " +
                    "VALUES (@KindID, @BehandelingID, @Naam, @Leeftijd)",
                    kind);
            }
        }

        public async Task<Kind?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Kind>(
                    "SELECT KindID, BehandelingID, Naam, Leeftijd FROM [Kind] WHERE KindID = @GUID",
                    new { GUID });
            }
        }

        public async Task<IEnumerable<Kind>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Kind>(
                    "SELECT KindID, BehandelingID, Naam, Leeftijd FROM [Kind]");
            }
        }

        public async Task UpdateAsync(Kind kind)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Kind] SET " +
                    "BehandelingID = @BehandelingID, " +
                    "Naam = @Naam, " +
                    "Leeftijd = @Leeftijd " +
                    "WHERE KindID = @KindID",
                    kind);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Kind] WHERE KindID = @GUID",
                    new { GUID });
            }
        }
    }
}
