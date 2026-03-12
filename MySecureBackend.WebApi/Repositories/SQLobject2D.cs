using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLobject2D : IObject2D
    {
        private readonly string sqlConnectionString;

        public SQLobject2D(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Object2D object2d)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
               await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (GUID, PrefabID, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentGUID) VALUES (@GUID, @PrefabID, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnviromentGUID)", object2d);
            }
        }

        public async Task<Object2D?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE GUID = @GUID", new { GUID });   
            }
        }

        public async Task<IEnumerable<Object2D>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task UpdateAsync(Object2D object2d)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Object2D] SET PrefabID = @PrefabID, PositionX = @PositionX, PositionY = @PositionY, " +
                    "ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer " +
                    "WHERE GUID = @GUID", object2d);
            }
        }

        public async Task<IEnumerable<Object2D>> SelectByEnvironmentAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>(
                    "SELECT * FROM [Object2D] WHERE EnvironmentGUID = @environmentId",
                    new { environmentId });
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE GUID = @GUID", new { GUID });
            }
        }

        public async Task DeleteByEnvironmentAsync(Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE EnvironmentGUID = @environmentId", new { environmentId });
            }
        }
    }
}
