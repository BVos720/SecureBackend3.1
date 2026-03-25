using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLSettingsRepository : ISettings
    {
        private readonly string sqlConnectionString;

        public SQLSettingsRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Settings settings)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Settings] (SettingsID, Character, Taal, Dyslexie, [Color Theme], KindID) " +
                    "VALUES (@SettingsID, @Character, @Taal, @Dyslexie, @ColorTheme, @KindID)",
                    settings);
            }
        }

        public async Task<Settings?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Settings>(
                    "SELECT SettingsID, Character, Taal, Dyslexie, [Color Theme] AS ColorTheme, KindID " +
                    "FROM [Settings] WHERE SettingsID = @GUID",
                    new { GUID });
            }
        }

        public async Task<Settings?> SelectByKindIdAsync(Guid kindID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Settings>(
                    "SELECT SettingsID, Character, Taal, Dyslexie, [Color Theme] AS ColorTheme, KindID " +
                    "FROM [Settings] WHERE KindID = @kindID",
                    new { kindID });
            }
        }

        public async Task<IEnumerable<Settings>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Settings>(
                    "SELECT SettingsID, Character, Taal, Dyslexie, [Color Theme] AS ColorTheme, KindID " +
                    "FROM [Settings]");
            }
        }

        public async Task UpdateAsync(Settings settings)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Settings] SET " +
                    "Character = @Character, " +
                    "Taal = @Taal, " +
                    "Dyslexie = @Dyslexie, " +
                    "[Color Theme] = @ColorTheme, " +
                    "KindID = @KindID " +
                    "WHERE SettingsID = @SettingsID",
                    settings);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Settings] WHERE SettingsID = @GUID",
                    new { GUID });
            }
        }
    }
}
