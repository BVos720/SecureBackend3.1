using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IOuder
    {
        Task InsertAsync(Ouder ouder);
        Task DeleteAsync(Guid GUID);
        Task<IEnumerable<Ouder>> SelectAsync();
        Task<Ouder?> SelectAsync(Guid GUID);
        Task<Ouder?> SelectByAccountIdAsync(string accountId);
        Task UpdateAsync(Ouder ouder);
    }
}
