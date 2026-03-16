using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IObject2D
    {

        Task InsertAsync(Models.Patient object2D);
        Task deleteAsync(Guid GUID);
        Task DeleteByEnvironmentAsync(Guid environmentId);
        Task<IEnumerable<Patient>> SelectAsync();
        Task<Patient?> SelectAsync(Guid GUID);
        Task UpdateAsync(Patient object2D);
        Task<IEnumerable<Patient>> SelectByEnvironmentAsync(Guid environmentId);

    }
}
