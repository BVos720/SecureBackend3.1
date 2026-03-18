using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLOuderRepository : IOuder
    {
        private readonly string sqlConnectionString;

        public SQLOuderRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Ouder ouder)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Ouder] (OuderID, Naam, AccountID) " +
                    "VALUES (@OuderID, @Naam, @AccountID)",
                    ouder);
            }
        }

        public async Task<Ouder?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Ouder>(
                    "SELECT OuderID, Naam, AccountID FROM [Ouder] WHERE OuderID = @GUID",
                    new { GUID });
            }
        }

        public async Task<Ouder?> SelectByAccountIdAsync(string accountId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Ouder>(
                    "SELECT OuderID, Naam, AccountID FROM [Ouder] WHERE AccountID = @accountId",
                    new { accountId });
            }
        }

        public async Task<IEnumerable<Ouder>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Ouder>(
                    "SELECT OuderID, Naam, AccountID FROM [Ouder]");
            }
        }

        public async Task UpdateAsync(Ouder ouder)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Ouder] SET " +
                    "Naam = @Naam, " +
                    "AccountID = @AccountID " +
                    "WHERE OuderID = @OuderID",
                    ouder);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Ouder] WHERE OuderID = @GUID",
                    new { GUID });
            }
        }
    }
}
