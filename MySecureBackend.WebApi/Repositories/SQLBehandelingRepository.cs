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
                    "INSERT INTO [Behandeling] (BehandelingID, Type, Datum, Arts, KindID) " +
                    "VALUES (@BehandelingID, @Type, @Datum, @Arts, @KindID)",
                    behandeling);
            }
        }

        public async Task<Behandeling?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Behandeling>(
                    "SELECT BehandelingID, Type, Datum, Arts, KindID FROM [Behandeling] WHERE BehandelingID = @GUID",
                    new { GUID });
            }
        }

        public async Task<Behandeling?> SelectByKindIdAsync(Guid kindID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Behandeling>(
                    "SELECT BehandelingID, Type, Datum, Arts, KindID FROM [Behandeling] WHERE KindID = @kindID",
                    new { kindID });
            }
        }

        public async Task<IEnumerable<Behandeling>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Behandeling>(
                    "SELECT BehandelingID, Type, Datum, Arts, KindID FROM [Behandeling]");
            }
        }

        public async Task UpdateAsync(Behandeling behandeling)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Behandeling] SET " +
                    "Type = @Type, " +
                    "Datum = @Datum, " +
                    "Arts = @Arts, " +
                    "KindID = @KindID " +
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
