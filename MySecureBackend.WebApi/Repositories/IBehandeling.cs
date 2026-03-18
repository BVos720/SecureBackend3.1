using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IBehandeling
    {
        Task InsertAsync(Behandeling behandeling);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<Behandeling>> SelectAsync();
        Task<Behandeling?> SelectAsync(Guid GUID);
        Task<Behandeling?> SelectByKindIdAsync(Guid kindID);
        Task UpdateAsync(Behandeling behandeling);
    }
}
