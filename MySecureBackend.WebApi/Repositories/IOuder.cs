using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IOuder
    {
        Task InsertAsync(Ouder ouder);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<Ouder>> SelectAsync();
        Task<Ouder?> SelectAsync(Guid GUID);
        Task UpdateAsync(Ouder ouder);
    }
}
