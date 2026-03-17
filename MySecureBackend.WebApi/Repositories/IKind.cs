using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IKind
    {
        Task InsertAsync(Kind kind);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<Kind>> SelectAsync();
        Task<Kind?> SelectAsync(Guid GUID);
        Task UpdateAsync(Kind kind);
    }
}
