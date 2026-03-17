using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLGameProgressRepository : IGameProgress
    {
        private readonly string sqlConnectionString;

        public SQLGameProgressRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(GameProgress gameProgress)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Game Progress] (GameProgressID, LevelProgress, Points) " +
                    "VALUES (@GameProgressID, @LevelProgress, @Points)",
                    gameProgress);
            }
        }

        public async Task<GameProgress?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<GameProgress>(
                    "SELECT GameProgressID, LevelProgress, Points FROM [Game Progress] WHERE GameProgressID = @GUID",
                    new { GUID });
            }
        }

        public async Task<IEnumerable<GameProgress>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<GameProgress>(
                    "SELECT GameProgressID, LevelProgress, Points FROM [Game Progress]");
            }
        }

        public async Task UpdateAsync(GameProgress gameProgress)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Game Progress] SET " +
                    "LevelProgress = @LevelProgress, " +
                    "Points = @Points " +
                    "WHERE GameProgressID = @GameProgressID",
                    gameProgress);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Game Progress] WHERE GameProgressID = @GUID",
                    new { GUID });
            }
        }
    }
}
