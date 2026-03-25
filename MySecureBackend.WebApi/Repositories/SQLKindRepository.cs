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
                    "INSERT INTO [Kind] (KindID, Naam, Leeftijd, OuderID) " +
                    "VALUES (@KindID, @Naam, @Leeftijd, @OuderID)",
                    kind);
            }
        }

        public async Task<Kind?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Kind>(
                    "SELECT KindID, Naam, Leeftijd, OuderID FROM [Kind] WHERE KindID = @GUID",
                    new { GUID });
            }
        }

        public async Task<Kind?> SelectByOuderIdAsync(Guid ouderID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Kind>(
                    "SELECT KindID, Naam, Leeftijd, OuderID FROM [Kind] WHERE OuderID = @ouderID",
                    new { ouderID });
            }
        }

        public async Task<IEnumerable<Kind>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Kind>(
                    "SELECT KindID, Naam, Leeftijd, OuderID FROM [Kind]");
            }
        }

        public async Task UpdateAsync(Kind kind)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Kind] SET " +
                    "Naam = @Naam, " +
                    "Leeftijd = @Leeftijd, " +
                    "OuderID = @OuderID " +
                    "WHERE KindID = @KindID",
                    kind);
            }
        }

        public async Task DeleteAsync(Guid GUID)
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
