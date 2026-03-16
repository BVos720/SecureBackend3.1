using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IPatient
    {

        Task InsertAsync(Models.Patient patient);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<Patient>> SelectAsync();
        Task<IEnumerable<Patient>> SelectByUserAsync(string userId);
        Task<Patient?> SelectAsync(Guid GUID);
        Task UpdateAsync(Patient patient);

    }
}
