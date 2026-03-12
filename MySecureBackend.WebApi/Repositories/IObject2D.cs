using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IObject2D
    {

        Task InsertAsync(Models.Object2D object2D);
        Task deleteAsync(Guid GUID);
        Task DeleteByEnvironmentAsync(Guid environmentId);
        Task<IEnumerable<Object2D>> SelectAsync();
        Task<Object2D?> SelectAsync(Guid GUID);
        Task UpdateAsync(Object2D object2D);
        Task<IEnumerable<Object2D>> SelectByEnvironmentAsync(Guid environmentId);

    }
}
