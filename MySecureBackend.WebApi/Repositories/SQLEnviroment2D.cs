using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLGameObjectRepository : ILevelProgress
    {
        private readonly string sqlConnectionString;

        public SQLGameObjectRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(LevelProgress levelProgress)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [LevelProgress] (LevelProgressId, Name, LevelProgressValue, Points, UserID) " +
                    "VALUES (@Id, @Name, @LevelProgressValue, @Points, @UserID)",
                    levelProgress);
            }
        }

        public async Task<LevelProgress?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT LevelProgressId AS Id, Name, LevelProgressValue, Points, UserID FROM [LevelProgress] WHERE LevelProgressId = @GUID";

                return await sqlConnection.QuerySingleOrDefaultAsync<LevelProgress>(sql, new { GUID = GUID.ToString() });
            }
        }

        public async Task<IEnumerable<LevelProgress>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT LevelProgressId AS Id, Name, LevelProgressValue, Points, UserID FROM [LevelProgress]";

                return await sqlConnection.QueryAsync<LevelProgress>(sql);
            }
        }

        public async Task<IEnumerable<LevelProgress>> SelectByUserAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string sql = "SELECT LevelProgressId AS Id, Name, LevelProgressValue, Points, UserID FROM [LevelProgress] WHERE UserID = @userId";

                return await sqlConnection.QueryAsync<LevelProgress>(sql, new { userId });
            }
        }

        public async Task UpdateAsync(LevelProgress levelProgress)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [LevelProgress] SET " +
                    "Name = @Name, " +
                    "LevelProgressValue = @LevelProgressValue, " +
                    "Points = @Points " +
                    "WHERE LevelProgressId = @Id",
                    levelProgress);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [LevelProgress] WHERE LevelProgressId = @GUID", new { GUID });
            }
        }

        public async Task DeleteByEnvironmentAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [LevelProgress] WHERE LevelProgressId = @environmentId", new { environmentId });
            }
        }
    }
}
