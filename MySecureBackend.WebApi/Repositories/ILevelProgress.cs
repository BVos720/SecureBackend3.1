using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface ILevelProgress
    {

        Task InsertAsync(Models.LevelProgress levelprogress);
        Task deleteAsync(Guid GUID);
        Task DeleteByEnvironmentAsync(Guid environmentId);
        Task<IEnumerable<LevelProgress>> SelectAsync();
        Task<IEnumerable<LevelProgress>> SelectByUserAsync(string userId);
        Task<LevelProgress?> SelectAsync(Guid GUID);
        Task UpdateAsync(LevelProgress levelprogress);

    }
}
