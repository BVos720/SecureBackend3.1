using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLGameObjectRepository : IEnviroment2D
    {
        private readonly string sqlConnectionString;

        public SQLGameObjectRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Enviroment2D enviroment2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Enviroment2D] (GUID, Name, MaxHeigth, MaxLenght, UserID) " +
                    "VALUES (@Id, @Name, @MaxHeight, @MaxLenght, @UserID)", 
                    enviroment2D);
            }
        }

        public async Task<Enviroment2D?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT GUID AS Id, Name, MaxHeigth AS MaxHeight, MaxLenght, UserID FROM [Enviroment2D] WHERE GUID = @GUID";

                return await sqlConnection.QuerySingleOrDefaultAsync<Enviroment2D>(sql, new { GUID = GUID.ToString() });
            }
        }

        public async Task<IEnumerable<Enviroment2D>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT GUID AS Id, Name, MaxHeigth AS MaxHeight, MaxLenght, UserID FROM [Enviroment2D]";

                return await sqlConnection.QueryAsync<Enviroment2D>(sql);
            }
        }

        public async Task<IEnumerable<Enviroment2D>> SelectByUserAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT GUID AS Id, Name, MaxHeigth AS MaxHeight, MaxLenght, UserID FROM [Enviroment2D] WHERE UserID = @userId";

                return await sqlConnection.QueryAsync<Enviroment2D>(sql, new { userId });
            }
        }

        public async Task UpdateAsync(Enviroment2D enviroment2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Enviroment2D] SET " +
                                                 "Name = @Name, " +
                                                 "MaxHeight = @MaxHeight " +
                                                 "MaxLenght = @MaxLenght" +
                                                 "WHERE GUID = @GUID", enviroment2D);

            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Enviroment2D] WHERE GUID = @GUID", new { GUID });
            }
        }
    }
}
