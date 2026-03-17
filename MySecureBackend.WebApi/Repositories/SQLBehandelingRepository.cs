using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLBehandelingRepository : IBehandeling
    {
        private readonly string sqlConnectionString;

        public SQLBehandelingRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Behandeling behandeling)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Behandeling] (BehandelingID, GameProgressID, Type, Datum) " +
                    "VALUES (@BehandelingID, @GameProgressID, @Type, @Datum)",
                    behandeling);
            }
        }

        public async Task<Behandeling?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Behandeling>(
                    "SELECT BehandelingID, GameProgressID, Type, Datum FROM [Behandeling] WHERE BehandelingID = @GUID",
                    new { GUID });
            }
        }

        public async Task<IEnumerable<Behandeling>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Behandeling>(
                    "SELECT BehandelingID, GameProgressID, Type, Datum FROM [Behandeling]");
            }
        }

        public async Task UpdateAsync(Behandeling behandeling)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Behandeling] SET " +
                    "GameProgressID = @GameProgressID, " +
                    "Type = @Type, " +
                    "Datum = @Datum " +
                    "WHERE BehandelingID = @BehandelingID",
                    behandeling);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Behandeling] WHERE BehandelingID = @GUID",
                    new { GUID });
            }
        }
    }
}
