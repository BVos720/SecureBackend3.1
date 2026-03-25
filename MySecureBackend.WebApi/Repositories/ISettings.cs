using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface ISettings
    {
        Task InsertAsync(Settings settings);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<Settings>> SelectAsync();
        Task<Settings?> SelectAsync(Guid GUID);
        Task<Settings?> SelectByKindIdAsync(Guid kindID);
        Task UpdateAsync(Settings settings);
    }
}
