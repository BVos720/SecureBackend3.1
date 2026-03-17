using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IGameProgress
    {
        Task InsertAsync(GameProgress gameProgress);
        Task deleteAsync(Guid GUID);
        Task<IEnumerable<GameProgress>> SelectAsync();
        Task<GameProgress?> SelectAsync(Guid GUID);
        Task UpdateAsync(GameProgress gameProgress);
    }
}
