using Dapper;
using Microsoft.Data.SqlClient;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SQLobject2D : IPatient
    {
        private readonly string sqlConnectionString;

        public SQLobject2D(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task InsertAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Patient] (PatientID, voornaam, achternaam, Leeftijd, UserID) " +
                    "VALUES (@PatientID, @voornaam, @achternaam, @Leeftijd, @UserID)",
                    patient);
            }
        }

        public async Task<Patient?> SelectAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Patient>(
                    "SELECT PatientID, voornaam, achternaam, Leeftijd FROM [Patient] WHERE PatientID = @GUID",
                    new { GUID });
            }
        }

        public async Task<IEnumerable<Patient>> SelectAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>(
                    "SELECT PatientID, voornaam, achternaam, Leeftijd FROM [Patient]");
            }
        }

        public async Task<IEnumerable<Patient>> SelectByUserAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Patient>(
                    "SELECT PatientID, voornaam, achternaam, Leeftijd FROM [Patient] WHERE UserID = @userId",
                    new { userId });
            }
        }

        public async Task UpdateAsync(Patient patient)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "UPDATE [Patient] SET " +
                    "voornaam = @voornaam, " +
                    "achternaam = @achternaam, " +
                    "Leeftijd = @Leeftijd " +
                    "WHERE PatientID = @PatientID",
                    patient);
            }
        }

        public async Task deleteAsync(Guid GUID)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Patient] WHERE PatientID = @GUID",
                    new { GUID });
            }
        }
    }
}
